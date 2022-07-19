namespace CSharpCAD;
using System.Text.Json;
using System.Collections.Concurrent;

public static partial class CSCAD
{
    private struct ViewData
    {
        public string title { get; set; }
        public List<int> color { get; set; }
        public List<List<double>> vertices { get; set; }
        public List<List<int>> faces { get; set; }
        public ViewData(string title, List<int> color, List<List<double>> vertices, List<List<int>> faces)
        {
            this.title = title;
            this.color = color;
            this.vertices = vertices;
            this.faces = faces;
        }
    }

    private static Thread? ProcessingThread = null;
    private static bool CADViewerInited = false;
    private static readonly HttpClient client = new HttpClient();
    private static ConcurrentQueue<HttpRequestMessage?> hrmQueue = new ConcurrentQueue<HttpRequestMessage?>();

    /// <summary>Use CADView protocol to display the geometry object.</summary>
    /// <remarks>Returns obj unchanged... so that it works well in return statements.</remarks>
    /// <example>
    ///   return View(someCalculationReturningGeom2);
    /// </example>
    /// <group>Miscellaneous</group>
    public static Geom2 View(Geom2 gobj, string title = "")
    {
        if (!GlobalParams.CADViewerEnabled) return (gobj);
        var g = ExtrudeLinear(gobj, height: 0.1);
        View(g, title);
        return gobj;
    }

    /// <summary>Use CADView protocol to display the geometry object.</summary>
    /// <remarks>Returns obj unchanged... so that it works well in return statements.</remarks>
    /// <example>
    ///   return View(someCalculationReturningGeom3);
    /// </example>
    /// <group>Miscellaneous</group>
    public static Geom3 View(Geom3 gobj, string title = "")
    {
        if (!GlobalParams.CADViewerEnabled) return (gobj);

        if (!CADViewerInited)
        {
            var hrm = new HttpRequestMessage(HttpMethod.Post, GlobalParams.CADViewerUrl);
            hrm.Content = new StringContent("{\"clear\":true}");
            hrmQueue.Enqueue(hrm);
            ProcessingThread = new Thread(new ThreadStart(Sender));
            ProcessingThread.IsBackground = true;
            ProcessingThread.Start();

            CADViewerInited = true;
        }

        var vertexMap = new Dictionary<Vec3, int>();
        var vertices = new List<List<double>>();
        var faces = new List<List<int>>();
        foreach (var poly in gobj.ToPolygons())
        {
            var face = new List<int>(poly.Vertices.Length);
            foreach (var v in poly.Vertices)
            {
                int v_idx = -1;
                if (vertexMap.ContainsKey(v))
                {
                    v_idx = vertexMap[v];
                }
                else
                {
                    v_idx = vertices.Count;
                    vertices.Add(new List<double> { v.X, v.Y, v.Z });
                    vertexMap[v] = v_idx;
                }
                face.Add(v_idx);
            }
            var count = face.Count;
            face.Insert(0, count);
            faces.Add(face);
        }
        var gColor = gobj.Color ?? new Color("tan");
        var color = new List<int> { (int)gColor.r, (int)gColor.g, (int)gColor.b };
        var vd = new ViewData(title, color, vertices, faces);
        var json = JsonSerializer.Serialize(vd);
        try
        {
            var hrm = new HttpRequestMessage(HttpMethod.Post, GlobalParams.CADViewerUrl);
            hrm.Content = new StringContent(json);
            hrmQueue.Enqueue(hrm);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("Exception preparing model to CADViewer, Message :{0} ", e.Message);
        }

        return (gobj);
    }

    ///<summary>Called to wait for CADViewer sending to finish.</summary>
    ///<param name="verbose">Show messages saying when waiting has started/finished.</param>
    ///<group>Miscellaneous</group>
    public static void WaitForViewerTransfers(bool verbose = true)
    {
        hrmQueue.Enqueue(null);
        if (verbose) Console.WriteLine("Waiting for viewer transfers.");
        if (ProcessingThread is not null) ProcessingThread.Join();
        if (verbose) Console.WriteLine("Finshed waiting for viewer transfers.");
    }

    private static void Sender()
    {
        for (; ; )
        {
            HttpRequestMessage? hrm = null;
            var hasItem = hrmQueue.TryDequeue(out hrm);
            if (!hasItem)
            {
                Thread.Sleep(1000);
                continue;
            }
            if (hrm is null) break;
            try
            {
                client.Send(hrm, HttpCompletionOption.ResponseContentRead);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Exception sending model to CADViewer, Message :{0} ", e.Message);
            }
        }
    }
}