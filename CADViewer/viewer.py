from pyvista import examples, Plotter
import pyvista
from time import sleep
import vtk
import sys
import server as serv

serv.start_server()

pl = Plotter()
current_mesh=-1
axes_on = True
show_edges = True
style="surface"

def do_nothing():
    pass

def exit_now():
    sys.exit(0)

def on_timer(iren, event_id):
    global current_mesh
    display_needed = False
    serv.view_lock.acquire()
    if serv.clear_needed:
        current_mesh = -1
        serv.meshes = []
        serv.colors = []
        serv.titles = []
        serv.clear_needed = False
        display_needed = True
    elif serv.new_mesh:
        serv.new_mesh = False
        current_mesh = -1
        display_needed = True
    serv.view_lock.release()
    if display_needed:
        display()

def display():
    global current_mesh
    serv.view_lock.acquire()
    pl.clear()
    pl.enable_trackball_style()
    pl.set_background("lightblue")
    pl.view_xy()
    pl.enable_lightkit()
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

def toggle_edges():
    global show_edges
    show_edges = not show_edges
    display()

def show_surface():
    global style
    if style != "surface":
        style="surface"
        display()

def show_points():
    global style
    if style != "points":
        style="points"
        display()

def show_wireframe():
    global style
    if style != "wireframe":
        style="wireframe"
        display()

while True:
    display()

    pl.add_key_event("a", toggle_axes)
    pl.add_key_event("minus", toggle_edges)
    pl.add_key_event("p", show_points)
    pl.add_key_event("w", show_wireframe)
    pl.add_key_event("s", show_surface)
    pl.add_key_event("Left", do_left)
    pl.add_key_event("Right", do_right)

    pl.render()

    pl.show(interactive=False, auto_close=False)
    id = pl.iren.interactor.CreateRepeatingTimer(1000)
    pl.iren.remove_observers("TimerEvent")
    pl.iren.interactor.AddObserver(vtk.vtkCommand.TimerEvent, on_timer)
    pl.show(interactive=True, auto_close=True)

    print("Leaving")
    sys.exit(0)
