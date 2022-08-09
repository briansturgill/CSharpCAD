We are looking at better ways to do this, but the CSharpCAD Python API is rather hard to install.

You will have to install the [dotnet 6.0 sdk](https://docs.microsoft.com/en-us/dotnet/core/install/linux) first.

We have placed the AppImages for pycscad and cadviewer here to make it easier for people to experiment with the Python API for CSharpCAD.
They are Linux AppImages. When you run them, the self-mount as FUSE filesystems and then execute. They contain all the files necessary to run themselves.
We made them using the [Python AppImage Project](https://github.com/niess/python-appimage).

Currently they are available only for Intel x86-64 version of Linux.

To "install" copy the two .Appimage files to a place in your path.
Generally you want to rename them to something more friendly.
For example on my own systems I do this:

cp pycscad.AppImage /usr/local/bin/pycscad
chmod +x /usr/local/bin/pycscad
cp cadviewer.AppImage /usr/local/bin/cadviewer
chmod +x /usr/local/bin/cadviewer

To get started with the Python API for CSharpCAD, you probably also want
`starter.py` and `testall.py` available in the [Python directory](https://github.com/briansturgill/CSharpCAD/tree/main/Python) of the main archive.
Also, the `pycscad.py` file there contains DocStrings for all of the functions.

If you use an editor that does autocomplete (like Visual Studio Code or PyCharm), then having pycscad.py in your working directory will enable that autocompletion.
(Otherwise, there is a copy of pycscad in the AppImage that gets loaded automatically via import.)

Workflow looks like this:
`cadviewer`
Do this in its own terminal window, when you type 'h' help will appear in that window. Also various other communication from the program. (Try taking a screen image capture with 'c'.)

Edit `your_pycscad_prog.py` file using `start.py` as a template.

To execute it:
`pycscad your_pycscad_prog.py`

The `view` and `save` calls in your program will cause 3d models to appear in cadviewer.

You will need to use the main
[CSharpCAD documentation](https://briansturgill.github.io/CSharpCAD/CSharpCADDocs.html)
for more detailed explainations of the functions.

The Python API uses
differently-cased names and uses Python lists `[...]` for `List<Vec2>`,
`List<Vec3`, etc. Also any CSCAD function that calls for a `Vec2` or
`Vec3` uses tuples in the Python API: `(10, 3.5)` or `(2.0, 3.8, 0)`.

Other equivalences:

For `null`, use `None`; `true`, `True`, `false`, `False`.

Any confusion can likely be resolved by searching for the CSCAD name in
`pycscad.py` and seeing what the corresponding Python wrapper function
signature is.
