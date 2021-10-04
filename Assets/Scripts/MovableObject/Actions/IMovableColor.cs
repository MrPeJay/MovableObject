using UnityEngine;

public interface IMovableColor
{
    Color Color { get; set; }
    Color PreviousColor { get; set; }
}