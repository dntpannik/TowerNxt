using Stride.Core.Presentation.Controls;
using Stride.Core.Presentation.Interop;
using Stride.Games;

namespace TowerNxt.WPF;

internal class EmbeddedGameForm : GameForm
{
    public EmbeddedGameForm()
    {
        enableFullscreenToggle = false;
    }

    /// <summary>
    /// Gets or sets the <see cref="GameEngineHost"/> associated to this form.
    /// </summary>
    public GameEngineHost Host { get; set; }

    /// <inheritdoc/>
    protected override void WndProc(ref System.Windows.Forms.Message m)
    {
        if (Host != null)
        {
            switch (m.Msg)
            {
                case NativeHelper.WM_KEYDOWN:
                case NativeHelper.WM_KEYUP:
                case NativeHelper.WM_MOUSEWHEEL:
                case NativeHelper.WM_RBUTTONDOWN:
                case NativeHelper.WM_RBUTTONUP:
                case NativeHelper.WM_LBUTTONDOWN:
                case NativeHelper.WM_LBUTTONUP:
                case NativeHelper.WM_MOUSEMOVE:
                case NativeHelper.WM_CONTEXTMENU:
                    Host.ForwardMessage(m.HWnd, m.Msg, m.WParam, m.LParam);
                    break;
            }
        }
        base.WndProc(ref m);
    }
}