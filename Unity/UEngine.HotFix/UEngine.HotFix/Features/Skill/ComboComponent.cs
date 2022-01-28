using System.Collections.Generic;
using Entitas;

[Unit]
public class ComboComponent : IComponent
{
    public int ComboIndex; //第几段连接
    public int PressedCount;//按压次数
    public bool IsComboFinish;//连击是否完成
}