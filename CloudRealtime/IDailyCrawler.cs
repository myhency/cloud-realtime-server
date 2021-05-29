using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CloudRealtime
{
    public interface IDailyCrawler
    {
        Button getRequestAllItemInfoButton();

        Button getRequestKospiItemInfoButton();

        Button getRequestKosdaqItemInfoButton();

        TextBox getGoogleSheetRowNumTextBox();

        List<string> getCodeList(string market);

        void requestOpt10001(string itemCode);

        void updateCodeListToGoogleSheet(Opt10001VO opt10001VO);
    }
}
