
namespace CloudRealtime
{
    partial class StockMiner
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StockMiner));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.googleSheetRowNumTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.requestAllItemInfoButton = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.boxTradeActivationCheckBox = new System.Windows.Forms.CheckBox();
            this.boxTradeEndRowTextBox = new System.Windows.Forms.TextBox();
            this.boxTradeStartRowTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.boxTradeEndColTextBox = new System.Windows.Forms.TextBox();
            this.boxTradeStartColTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.swingActivationCheckBox = new System.Windows.Forms.CheckBox();
            this.shortTermEndRowTextBox = new System.Windows.Forms.TextBox();
            this.shortTermStartRowTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.shortTermEndColTextBox = new System.Windows.Forms.TextBox();
            this.shortTermStartColTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.longTermActivationCheckBox = new System.Windows.Forms.CheckBox();
            this.longTermEndRowTextBox = new System.Windows.Forms.TextBox();
            this.longTermStartRowTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.longTermEndColTextBox = new System.Windows.Forms.TextBox();
            this.longTermStartColTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.startAlarmButton = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.axKHOpenAPI1 = new AxKHOpenAPILib.AxKHOpenAPI();
            this.requestKospiItemInfoButton = new System.Windows.Forms.Button();
            this.requestKosdaqItemInfoButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(502, 372);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.requestKosdaqItemInfoButton);
            this.tabPage1.Controls.Add(this.requestKospiItemInfoButton);
            this.tabPage1.Controls.Add(this.googleSheetRowNumTextBox);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.requestAllItemInfoButton);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(494, 346);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "데일리종목정보";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // googleSheetRowNumTextBox
            // 
            this.googleSheetRowNumTextBox.Location = new System.Drawing.Point(216, 97);
            this.googleSheetRowNumTextBox.Name = "googleSheetRowNumTextBox";
            this.googleSheetRowNumTextBox.Size = new System.Drawing.Size(100, 21);
            this.googleSheetRowNumTextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "재시작시 시작번호(구글시트 행번호)";
            // 
            // requestAllItemInfoButton
            // 
            this.requestAllItemInfoButton.Location = new System.Drawing.Point(6, 6);
            this.requestAllItemInfoButton.Name = "requestAllItemInfoButton";
            this.requestAllItemInfoButton.Size = new System.Drawing.Size(150, 23);
            this.requestAllItemInfoButton.TabIndex = 0;
            this.requestAllItemInfoButton.Text = "전 종목 정보 가져오기";
            this.requestAllItemInfoButton.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.startAlarmButton);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(494, 346);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "알리미";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.boxTradeActivationCheckBox);
            this.groupBox3.Controls.Add(this.boxTradeEndRowTextBox);
            this.groupBox3.Controls.Add(this.boxTradeStartRowTextBox);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.boxTradeEndColTextBox);
            this.groupBox3.Controls.Add(this.boxTradeStartColTextBox);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Location = new System.Drawing.Point(6, 167);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(232, 143);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "박스권종목 시트 설정";
            // 
            // boxTradeActivationCheckBox
            // 
            this.boxTradeActivationCheckBox.AutoSize = true;
            this.boxTradeActivationCheckBox.Checked = true;
            this.boxTradeActivationCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.boxTradeActivationCheckBox.Location = new System.Drawing.Point(12, 116);
            this.boxTradeActivationCheckBox.Name = "boxTradeActivationCheckBox";
            this.boxTradeActivationCheckBox.Size = new System.Drawing.Size(60, 16);
            this.boxTradeActivationCheckBox.TabIndex = 8;
            this.boxTradeActivationCheckBox.Text = "활성화";
            this.boxTradeActivationCheckBox.UseVisualStyleBackColor = true;
            // 
            // boxTradeEndRowTextBox
            // 
            this.boxTradeEndRowTextBox.Location = new System.Drawing.Point(120, 85);
            this.boxTradeEndRowTextBox.Name = "boxTradeEndRowTextBox";
            this.boxTradeEndRowTextBox.Size = new System.Drawing.Size(100, 21);
            this.boxTradeEndRowTextBox.TabIndex = 7;
            // 
            // boxTradeStartRowTextBox
            // 
            this.boxTradeStartRowTextBox.Location = new System.Drawing.Point(12, 85);
            this.boxTradeStartRowTextBox.Name = "boxTradeStartRowTextBox";
            this.boxTradeStartRowTextBox.Size = new System.Drawing.Size(100, 21);
            this.boxTradeStartRowTextBox.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(120, 69);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 12);
            this.label10.TabIndex = 5;
            this.label10.Text = "마지막행 번호";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 69);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(69, 12);
            this.label11.TabIndex = 4;
            this.label11.Text = "시작행 번호";
            // 
            // boxTradeEndColTextBox
            // 
            this.boxTradeEndColTextBox.Location = new System.Drawing.Point(120, 37);
            this.boxTradeEndColTextBox.Name = "boxTradeEndColTextBox";
            this.boxTradeEndColTextBox.Size = new System.Drawing.Size(100, 21);
            this.boxTradeEndColTextBox.TabIndex = 3;
            // 
            // boxTradeStartColTextBox
            // 
            this.boxTradeStartColTextBox.Location = new System.Drawing.Point(9, 37);
            this.boxTradeStartColTextBox.Name = "boxTradeStartColTextBox";
            this.boxTradeStartColTextBox.Size = new System.Drawing.Size(100, 21);
            this.boxTradeStartColTextBox.TabIndex = 2;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(120, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(93, 12);
            this.label12.TabIndex = 1;
            this.label12.Text = "마지막컬럼 번호";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(81, 12);
            this.label13.TabIndex = 0;
            this.label13.Text = "시작컬럼 번호";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.swingActivationCheckBox);
            this.groupBox2.Controls.Add(this.shortTermEndRowTextBox);
            this.groupBox2.Controls.Add(this.shortTermStartRowTextBox);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.shortTermEndColTextBox);
            this.groupBox2.Controls.Add(this.shortTermStartColTextBox);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Location = new System.Drawing.Point(244, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(232, 142);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "스윙종목 시트 설정";
            // 
            // swingActivationCheckBox
            // 
            this.swingActivationCheckBox.AutoSize = true;
            this.swingActivationCheckBox.Checked = true;
            this.swingActivationCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.swingActivationCheckBox.Location = new System.Drawing.Point(12, 115);
            this.swingActivationCheckBox.Name = "swingActivationCheckBox";
            this.swingActivationCheckBox.Size = new System.Drawing.Size(60, 16);
            this.swingActivationCheckBox.TabIndex = 8;
            this.swingActivationCheckBox.Text = "활성화";
            this.swingActivationCheckBox.UseVisualStyleBackColor = true;
            // 
            // shortTermEndRowTextBox
            // 
            this.shortTermEndRowTextBox.Location = new System.Drawing.Point(120, 85);
            this.shortTermEndRowTextBox.Name = "shortTermEndRowTextBox";
            this.shortTermEndRowTextBox.Size = new System.Drawing.Size(100, 21);
            this.shortTermEndRowTextBox.TabIndex = 7;
            // 
            // shortTermStartRowTextBox
            // 
            this.shortTermStartRowTextBox.Location = new System.Drawing.Point(12, 85);
            this.shortTermStartRowTextBox.Name = "shortTermStartRowTextBox";
            this.shortTermStartRowTextBox.Size = new System.Drawing.Size(100, 21);
            this.shortTermStartRowTextBox.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(120, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "마지막행 번호";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 12);
            this.label7.TabIndex = 4;
            this.label7.Text = "시작행 번호";
            // 
            // shortTermEndColTextBox
            // 
            this.shortTermEndColTextBox.Location = new System.Drawing.Point(120, 37);
            this.shortTermEndColTextBox.Name = "shortTermEndColTextBox";
            this.shortTermEndColTextBox.Size = new System.Drawing.Size(100, 21);
            this.shortTermEndColTextBox.TabIndex = 3;
            // 
            // shortTermStartColTextBox
            // 
            this.shortTermStartColTextBox.Location = new System.Drawing.Point(9, 37);
            this.shortTermStartColTextBox.Name = "shortTermStartColTextBox";
            this.shortTermStartColTextBox.Size = new System.Drawing.Size(100, 21);
            this.shortTermStartColTextBox.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(120, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 12);
            this.label8.TabIndex = 1;
            this.label8.Text = "마지막컬럼 번호";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "시작컬럼 번호";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.longTermActivationCheckBox);
            this.groupBox1.Controls.Add(this.longTermEndRowTextBox);
            this.groupBox1.Controls.Add(this.longTermStartRowTextBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.longTermEndColTextBox);
            this.groupBox1.Controls.Add(this.longTermStartColTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 142);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "중장기종목 시트 설정";
            // 
            // longTermActivationCheckBox
            // 
            this.longTermActivationCheckBox.AutoSize = true;
            this.longTermActivationCheckBox.Checked = true;
            this.longTermActivationCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.longTermActivationCheckBox.Location = new System.Drawing.Point(12, 116);
            this.longTermActivationCheckBox.Name = "longTermActivationCheckBox";
            this.longTermActivationCheckBox.Size = new System.Drawing.Size(60, 16);
            this.longTermActivationCheckBox.TabIndex = 8;
            this.longTermActivationCheckBox.Text = "활성화";
            this.longTermActivationCheckBox.UseVisualStyleBackColor = true;
            // 
            // longTermEndRowTextBox
            // 
            this.longTermEndRowTextBox.Location = new System.Drawing.Point(120, 85);
            this.longTermEndRowTextBox.Name = "longTermEndRowTextBox";
            this.longTermEndRowTextBox.Size = new System.Drawing.Size(100, 21);
            this.longTermEndRowTextBox.TabIndex = 7;
            // 
            // longTermStartRowTextBox
            // 
            this.longTermStartRowTextBox.Location = new System.Drawing.Point(12, 85);
            this.longTermStartRowTextBox.Name = "longTermStartRowTextBox";
            this.longTermStartRowTextBox.Size = new System.Drawing.Size(100, 21);
            this.longTermStartRowTextBox.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(120, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "마지막행 번호";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "시작행 번호";
            // 
            // longTermEndColTextBox
            // 
            this.longTermEndColTextBox.Location = new System.Drawing.Point(120, 37);
            this.longTermEndColTextBox.Name = "longTermEndColTextBox";
            this.longTermEndColTextBox.Size = new System.Drawing.Size(100, 21);
            this.longTermEndColTextBox.TabIndex = 3;
            // 
            // longTermStartColTextBox
            // 
            this.longTermStartColTextBox.Location = new System.Drawing.Point(9, 37);
            this.longTermStartColTextBox.Name = "longTermStartColTextBox";
            this.longTermStartColTextBox.Size = new System.Drawing.Size(100, 21);
            this.longTermStartColTextBox.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(120, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "마지막컬럼 번호";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "시작컬럼 번호";
            // 
            // startAlarmButton
            // 
            this.startAlarmButton.Location = new System.Drawing.Point(6, 316);
            this.startAlarmButton.Name = "startAlarmButton";
            this.startAlarmButton.Size = new System.Drawing.Size(75, 23);
            this.startAlarmButton.TabIndex = 0;
            this.startAlarmButton.Text = "알리미켜기";
            this.startAlarmButton.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox5);
            this.tabPage3.Controls.Add(this.groupBox4);
            this.tabPage3.Controls.Add(this.textBox1);
            this.tabPage3.Controls.Add(this.label14);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(494, 346);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "매매설정";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBox6);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Controls.Add(this.textBox7);
            this.groupBox5.Controls.Add(this.label20);
            this.groupBox5.Controls.Add(this.textBox8);
            this.groupBox5.Controls.Add(this.label21);
            this.groupBox5.Controls.Add(this.textBox9);
            this.groupBox5.Controls.Add(this.label22);
            this.groupBox5.Location = new System.Drawing.Point(167, 51);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(151, 135);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "차수별 매도 비중";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(37, 102);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(100, 21);
            this.textBox6.TabIndex = 7;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(7, 106);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(23, 12);
            this.label19.TabIndex = 6;
            this.label19.Text = "4차";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(37, 75);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(100, 21);
            this.textBox7.TabIndex = 5;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(7, 79);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(23, 12);
            this.label20.TabIndex = 4;
            this.label20.Text = "3차";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(37, 48);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(100, 21);
            this.textBox8.TabIndex = 3;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(7, 52);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(23, 12);
            this.label21.TabIndex = 2;
            this.label21.Text = "2차";
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(37, 21);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(100, 21);
            this.textBox9.TabIndex = 1;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(7, 25);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(23, 12);
            this.label22.TabIndex = 0;
            this.label22.Text = "1차";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBox5);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.textBox4);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.textBox3);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.textBox2);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Location = new System.Drawing.Point(10, 51);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(151, 135);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "차수별 매수 비중";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(37, 102);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 21);
            this.textBox5.TabIndex = 7;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(7, 106);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(23, 12);
            this.label18.TabIndex = 6;
            this.label18.Text = "4차";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(37, 75);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 21);
            this.textBox4.TabIndex = 5;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(7, 79);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(23, 12);
            this.label17.TabIndex = 4;
            this.label17.Text = "3차";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(37, 48);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 21);
            this.textBox3.TabIndex = 3;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(7, 52);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(23, 12);
            this.label16.TabIndex = 2;
            this.label16.Text = "2차";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(37, 21);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 21);
            this.textBox2.TabIndex = 1;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(7, 25);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(23, 12);
            this.label15.TabIndex = 0;
            this.label15.Text = "1차";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(10, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(129, 21);
            this.textBox1.TabIndex = 1;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 8);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(131, 12);
            this.label14.TabIndex = 0;
            this.label14.Text = "최대 매수 금액(종목별)";
            // 
            // axKHOpenAPI1
            // 
            this.axKHOpenAPI1.Enabled = true;
            this.axKHOpenAPI1.Location = new System.Drawing.Point(652, 29);
            this.axKHOpenAPI1.Name = "axKHOpenAPI1";
            this.axKHOpenAPI1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKHOpenAPI1.OcxState")));
            this.axKHOpenAPI1.Size = new System.Drawing.Size(83, 10);
            this.axKHOpenAPI1.TabIndex = 1;
            // 
            // requestKospiItemInfoButton
            // 
            this.requestKospiItemInfoButton.Location = new System.Drawing.Point(6, 35);
            this.requestKospiItemInfoButton.Name = "requestKospiItemInfoButton";
            this.requestKospiItemInfoButton.Size = new System.Drawing.Size(150, 23);
            this.requestKospiItemInfoButton.TabIndex = 3;
            this.requestKospiItemInfoButton.Text = "Kospi 정보 가져오기";
            this.requestKospiItemInfoButton.UseVisualStyleBackColor = true;
            // 
            // requestKosdaqItemInfoButton
            // 
            this.requestKosdaqItemInfoButton.Location = new System.Drawing.Point(6, 64);
            this.requestKosdaqItemInfoButton.Name = "requestKosdaqItemInfoButton";
            this.requestKosdaqItemInfoButton.Size = new System.Drawing.Size(150, 23);
            this.requestKosdaqItemInfoButton.TabIndex = 4;
            this.requestKosdaqItemInfoButton.Text = "Kosdaq 정보 가져오기";
            this.requestKosdaqItemInfoButton.UseVisualStyleBackColor = true;
            // 
            // StockMiner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 396);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.axKHOpenAPI1);
            this.Name = "StockMiner";
            this.Text = "올빼미 v0.1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button requestAllItemInfoButton;
        private System.Windows.Forms.TabPage tabPage2;
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private System.Windows.Forms.TextBox googleSheetRowNumTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button startAlarmButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox longTermEndRowTextBox;
        private System.Windows.Forms.TextBox longTermStartRowTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox longTermEndColTextBox;
        private System.Windows.Forms.TextBox longTermStartColTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox shortTermEndRowTextBox;
        private System.Windows.Forms.TextBox shortTermStartRowTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox shortTermEndColTextBox;
        private System.Windows.Forms.TextBox shortTermStartColTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox boxTradeActivationCheckBox;
        private System.Windows.Forms.TextBox boxTradeEndRowTextBox;
        private System.Windows.Forms.TextBox boxTradeStartRowTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox boxTradeEndColTextBox;
        private System.Windows.Forms.TextBox boxTradeStartColTextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox swingActivationCheckBox;
        private System.Windows.Forms.CheckBox longTermActivationCheckBox;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button requestKosdaqItemInfoButton;
        private System.Windows.Forms.Button requestKospiItemInfoButton;
    }
}

