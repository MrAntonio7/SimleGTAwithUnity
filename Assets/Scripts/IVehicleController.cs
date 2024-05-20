using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVehicleController
{
    bool IsActiveAndEnabled { get; }
    void ActivarVehiculo();
    void DesactivarVehiculo();

    void ActivarCanvasControles();
    void DesactivarCanvasControles();
    void ReiniciarVehiculo();
}
