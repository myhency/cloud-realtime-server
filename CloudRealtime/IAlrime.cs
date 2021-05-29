using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CloudRealtime
{
    public interface IAlrime
    {
        Button getStartAlarmButton();

        TextBox getLongTermStartColTextBox();

        TextBox getLongTermEndColTextBox();

        TextBox getLongTermStartRowTextBox();

        TextBox getLongTermEndRowTextBox();

        TextBox getShortTermStartColTextBox();

        TextBox getShortTermEndColTextBox();

        TextBox getShortTermStartRowTextBox();

        TextBox getShortTermEndRowTextBox();

        TextBox getBoxTradeStartColTextBox();

        TextBox getBoxTradeEndColTextBox();

        TextBox getBoxTradeStartRowTextBox();

        TextBox getBoxTradeEndRowTextBox();

        CheckBox getLongTermActivationCheckBox();

        CheckBox getSwingActivationCheckBox();

        CheckBox getBoxTradeActivationCheckBox();
    }
}
