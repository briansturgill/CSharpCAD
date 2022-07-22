python3 -m venv .venv
call .venv/Scripts/activate.bat
cd .venv
git clone https://github.com/pythonnet/pythonnet.git
cd pythonnet
pip install .
cd ..\..