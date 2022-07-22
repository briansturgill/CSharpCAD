# This is the Python API for CSharpCAD.

We recommend you using an editor (such as VS Code) that does auto
completion for Python. The pycscad.py file contains appropriate docstrings.

Sometimes you may need to consult the CSharpCAD documentation for more
information, it is at:
	https://briansturgill.github.io/CSharpCAD/CSharpCADDocs.html

To use pycscad, run the setup script (to create a .venv directory containing
the needed packages). See Setup below.

You will also want to install the CADViewer in the directory above.

You can then use it like this:

pycscad starter.py

Also, this command will send 48 test images to your CADViewer.

pycscad testall.py

Looking at testall.py can give you an overview of what is available.


## Setup
The Windows setup script assumes you are using the Python installed
from the Microsoft Store.
It also assumes you have Python for Windows installed from:
	https://git-scm.com/

You must be able to start git and python3 from a command prompt.
You then just run setup.bat
