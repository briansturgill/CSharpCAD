from pyvista import examples, Plotter
import pyvista
from time import sleep
import vtk
import sys
import server as serv
from pathlib import Path
import tempfile
from datetime import datetime

serv.start_server()

pl = Plotter()
current_mesh=-1
axes_on = True
show_bounds = False
show_edges = True
style="surface"

def do_nothing():
    pass

def exit_now():
    sys.exit(0)

def on_timer(iren, event_id):
    global current_mesh

    if serv.display_needed:
        current_mesh = -1
        display()

def display():
    global current_mesh
    serv.view_lock.acquire()
    serv.display_needed = False
    pl.clear()
    #pl.enable_trackball_style()
    if show_bounds:
        pl.show_bounds(color="black", grid=True, ticks="outside", font_size=8)
    pl.set_background("lightblue")
    pl.view_xz()
    pl.camera.azimuth = 45
    pl.camera.elevation = 45
    pl.enable_3_lights()
    pl.enable_anti_aliasing()
    if axes_on:
        pl.add_axes()
    l = len(serv.meshes)
    if (l > 0):
        if current_mesh < 0:
            current_mesh = l-1;
        elif current_mesh >= l:
            current_mesh = 0;
        ctmp = serv.colors[current_mesh]
        color = [ctmp[0]/255.0, ctmp[1]/255.0, ctmp[2]/255.0];
        if style == "points":
            color="black"
            pl.set_background("white")
        pl.add_mesh(serv.meshes[current_mesh], color=color, show_edges=show_edges, edge_color="brown", style=style)
        pl.reset_camera()
        title = serv.titles[current_mesh]
        if title != "": title = " - " + title
    else:
        current_mesh = -1;
        title = ""
    text_actor = pl.add_text(f"#{current_mesh+1} of {l} {title}", color="royalblue")
    pl.add_text("Type h - for help", font_size=10,
            position = "lower_right", color="royalblue")
    pl.render()
    serv.view_lock.release()

def do_left():
    global current_mesh
    current_mesh -= 1;
    display()

def do_right():
    global current_mesh
    current_mesh += 1;
    display()

def toggle_axes():
    global axes_on
    axes_on = not axes_on
    if axes_on:
        pl.add_axes()
    else:
        pl.hide_axes()
    pl.render()

def toggle_bounds():
    global show_bounds
    show_bounds = not show_bounds
    display()

def toggle_edges():
    global show_edges
    show_edges = not show_edges
    display()

def show_surface():
    global style
    if style != "surface":
        style="surface"
        display()

def toggle_points():
    global style
    if style != "points":
        style="points"
    else:
        style="surface"
    display()

def toggle_wireframe():
    global style
    if style != "wireframe":
        style="wireframe"
    else:
        style="surface"
    display()

def screen_capture():
    now = datetime.now().isoformat()
    file=Path(tempfile.gettempdir(), f"CADViewer_snap{now}.png");
    pl.screenshot(file)
    print(f"Screen shot saved to: {file}")

def show_help():
    print("Mouse left button held down and dragged meanings:")
    print("--------")
    print("No modifier: Rotate item being viewed.")
    print("Shift: Move model being viewed.")
    print("Ctrl: Spin model being viewed.")
    print("Ctrl Shift : Scale model being viewed.")
    print("--------")
    print("Key definitions:")
    print("--------")
    print("The left arrow and right arrows switch item being viewed.")
    print("a - toggle axes")
    print("b - toggle bounds")
    print("c - capture image of current screen")
    print("e - exit")
    print("h - help")
    print("p - show points")
    print("q - exit")
    print("s - show surfaces (default)")
    print("w - show wireframe")
    print("minus (-) - toggle edges")

while True:
    pl.add_key_event("a", toggle_axes)
    pl.add_key_event("b", toggle_bounds)
    pl.add_key_event("c", screen_capture)
    pl.add_key_event("h", show_help)
    pl.add_key_event("minus", toggle_edges)
    pl.add_key_event("p", toggle_points)
    pl.add_key_event("w", toggle_wireframe)
    pl.add_key_event("s", show_surface)
    pl.add_key_event("Left", do_left)
    pl.add_key_event("Right", do_right)

    pl.show(interactive=False, auto_close=False)
    id = pl.iren.interactor.CreateRepeatingTimer(50)
    pl.iren.remove_observers("TimerEvent")
    pl.iren.interactor.AddObserver(vtk.vtkCommand.TimerEvent, on_timer)
    display()
    pl.show(interactive=True, auto_close=True)

    print("CADView exiting.")
    sys.exit(0)
