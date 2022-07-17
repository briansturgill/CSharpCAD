import pyvista
from http.server import HTTPServer, BaseHTTPRequestHandler
from threading import Thread, Lock
from queue import Queue
import json
import numpy as np
from time import sleep

view_lock = Lock()
display_needed = False
meshes = []
colors = []
titles = []

addr="127.0.0.1"
port=8037

viewQueue = Queue()

class CADViewerHandler(BaseHTTPRequestHandler):
    def do_POST(self):
        len = int(self.headers["Content-Length"])
        print("Content-Length:", len)
        data = self.rfile.read(len)
        self.send_response(200)
        self.end_headers()
        self.wfile.write(bytes("got it", "utf-8"))
        viewQueue.put(data)

def _queue_handler():
    global display_needed, meshes, colors, titles
    while True:
        data = viewQueue.get()
        view_lock.acquire()
        try:
            obj = json.loads(data)
            if "clear" in obj:
                meshes = []
                colors = []
                titles = []
                display_needed = True
            else:
                display_needed = True
                vertices = np.array(obj["vertices"], np.float64)
                faces = np.hstack(obj["faces"])
                mesh = pyvista.PolyData(vertices, faces)
                meshes.append(mesh)
                colors.append(obj["color"])
                titles.append(obj["title"])
        except Exception as err:
            print("Exception:", err)
            pass
        view_lock.release()

def start_server():
    Thread(target=_start_server, daemon=True).start()
    Thread(target=_queue_handler, daemon=True).start()

def _start_server():
    print(addr, port)
    httpd = HTTPServer((addr, port), CADViewerHandler)
    try:
        httpd.serve_forever()
    except KeyboardInterrupt:
        pass
    httpd.server_close()

if __name__ == "__main__":
    start_server()
    print("^C to exit.")
    while True:
        try:
            input("")
        except KeyboardInterrupt:
            break
