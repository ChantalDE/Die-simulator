using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

/*
 * The following correction have been made:
 * 1) changed updateVal to a float data type, so now it allows to validate the condition to update quick in real time
 * 2) corrected the math and graph for SUM of 2 DIE function
 * 3) set the seed to default 999 when RESET is hit
 * 4) corrected min value of Sum of 2 DIE so it would not say that 1 is the lowest throw
 */

namespace project1
{
    public partial class Form1 : Form
    {
        
        //global initiated die objects
        private Die die1;
        private Die die2;

        /// <summary>
        /// set the labels back to blank
        /// </summary>

        private void resetDie1()
        {
            Die1MeanL.Text = "";
            minCountDie1L.Text = "";
            maxCountDie1L.Text = "";
            fMinCountDie1L.Text = "";
            fMaxCountDie1L.Text = "";
        }
        private void resetDie2()
        {
            Die2MeanL.Text = "";
            minCountDie2L.Text = "";
            maxCountDie2L.Text = "";
            fMinCountDie2L.Text = "";
            fMaxCountDie2L.Text = "";
        }
        private void resetSum()
        {
            Sum2DiceMean.Text = "";
            minSumCountL.Text = "";
            fminSumCountL.Text = "";
            maxSumCountL.Text = "";
            fmaxSumCountL.Text = "";
        }

        /// <summary>
        /// Initialization of the form:
        /// By default, the text has 999
        /// This is being passed to the random object within the die constructor.
        /// Since the rand variable in the RandNum class is static, this is the random 
        /// sequence that tie die will follow, unless the default constructor is triggered.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            die1 = new Die(Int16.Parse(seedBox.Text));
            die2 = new Die(Int16.Parse(seedBox.Text));
        }

        private void Form1_Load(object sender, EventArgs e){}


        //roll Button
        //just rolls the dice
        private void button1_Click(object sender, EventArgs e)
        {
            //reset all stats
            resetDie1();
            resetDie2();
            resetSum();

            //place image
            die1F.Image = die1.diePics[die1.Roll() - 1];
            die2F.Image = die2.diePics[die2.Roll() - 1];
        }

                //statistics button
        /// <summary>
        /// This function triggers both die to run x amount of times selected by the user.
        /// Each die will calculate:
        ///     - the mean of the die
        ///     - The face that appeared the most
        ///     - The # of times of the max face appeared
        ///     - The face that appeared the most
        ///     - The # of tiems the min face appeared
        /// </summary>
        /// 
        private void button2_Click(object sender, EventArgs e)
        {
            //reset sum
            resetSum();

            //if the seedbox is empty, the default constructor of the random object will be used
            if (String.IsNullOrEmpty(seedBox.Text))
            {
                die1 = new Die();
                die2 = new Die();
            }
            //else, seed number is placd in, is what will be used for the random object.
            else
            {
                die1 = new Die(Int16.Parse(seedBox.Text));
                die2 = new Die(Int16.Parse(seedBox.Text));
            }

            //reset all items first
            foreach (var series in statisticsChart.Series)
            {
                series.Points.Clear();
            }

            //holds num of rolls in array
            int[] rollsDie1 = new int[6];
            int[] rollsDie2 = new int[6];

            //roll #times from textBoX
            float numRolls = Int32.Parse(statisticsCB.SelectedItem.ToString());
            
            float updateVal = 1;
            float sum1 = 0;
            float sum2 = 0;
            for (int i = 0; i < numRolls; i++)
            {
                int face1 = die1.Roll();
                int face2 = die2.Roll();

                //calculate sum for mean
                sum1 += face1;
                sum2 += face2;


                //display and update histogram
                statisticsChart.Series["Die1"].Points.AddXY(face1, ++rollsDie1[face1-1]);
                statisticsChart.Series["Die2"].Points.AddXY(face2, ++rollsDie2[face2-1]);

                //update val 
                if (updateVal == (numRolls/10))
                {
                    statisticsChart.Update();
                    updateVal = 0;
                }
                updateVal++;
            }

            //********************calculate and Display Die1*********************
            float meanDie1 = sum1 / (numRolls);
            Die1MeanL.Text = meanDie1.ToString();

            //retrieve and set minCount
            int minValueDie1 = rollsDie1.Min();
            minCountDie1L.Text = minValueDie1.ToString();

            //retrieve and set minFace
            int minFaceDie1 = rollsDie1.ToList().IndexOf(minValueDie1) + 1;
            fMinCountDie1L.Text = minFaceDie1.ToString();

            //retrieve and set MaxCount
            int maxValueDie1 = rollsDie1.Max();
            maxCountDie1L.Text = maxValueDie1.ToString();

            //retrieve and set minFace
            int maxFaceDie1 = rollsDie1.ToList().IndexOf(maxValueDie1) + 1;
            fMaxCountDie1L.Text = maxFaceDie1.ToString();


            //**********************calculate and Display Die2*****************
            float meanDie2 = sum2 / (numRolls);
            Die2MeanL.Text = meanDie2.ToString();

            //retrieve and set minCount
            int minValueDie2 = rollsDie2.Min();
            minCountDie2L.Text = minValueDie2.ToString();

            //retrieve and set minFace
            int minFaceDie2 = rollsDie2.ToList().IndexOf(minValueDie2) + 1;
            fMinCountDie2L.Text = minFaceDie2.ToString();

            //retrieve and set MaxCount
            int maxValueDie2 = rollsDie2.Max();
            maxCountDie2L.Text = maxValueDie2.ToString();

            //retrieve and set minFace
            int maxFaceDie2 = rollsDie2.ToList().IndexOf(maxValueDie2) + 1;
            fMaxCountDie2L.Text = maxFaceDie2.ToString();
        }

        //Reset Button
        /// <summary>
        /// resets all stats, clears the chart and sets the die back.
        /// </summary>
        private void resetB_Click(object sender, EventArgs e)
        {
            //seed is empty
            seedBox.Text = "999";

            //reinstantiate
            die1 = new Die(999);
            die2 = new Die(999);

            //reset stats
            resetDie1();
            resetDie2();
            resetSum();

            //reset histogram
            foreach (var series in statisticsChart.Series)
            {
                series.Points.Clear();
            }

            //reset die
            die1F.Image = Properties.Resources.static_die;
            die2F.Image = Properties.Resources.static_die;
        }


        /// <summary>
        ///                 Sum of 2 die.
        ///Calculates and displays the the mean of the sum of the die
        ///displays what face of dice showed up least,
        ///displays what face of the die showed up most
        /// </summary>
        private void SumOf2DiceB_Click(object sender, EventArgs e)
        {
            resetDie1();
            resetDie2();

            //initialize Dice
            if (String.IsNullOrEmpty(seedBox.Text))
            {
                die1 = new Die();
                die2 = new Die();
            }
            else
            {
                die1 = new Die(Int16.Parse(seedBox.Text));
                die2 = new Die(Int16.Parse(seedBox.Text));
            }

            //reset all chart first
            foreach (var series in statisticsChart.Series)
            {
                series.Points.Clear();
            }

            //holds num of rolls in array
            int[] totalRolls = new int[12];

            //roll #times from textBoX
            float numRolls = Int32.Parse(statisticsCB.SelectedItem.ToString());
            //CORRECTED UPDATE VAL FROM INT TO FLOAT
            float updateVal = 1;
            float sum = 0;
            for (int i = 0; i < numRolls; i++)
            {
                int face1 = die1.Roll();
                int face2 = die2.Roll();
                int dice = face1+face2;

                //update Rolls
                ++totalRolls[dice - 1];

                //calculate sum for mean
                sum += (face1 + face2);

                //display and update histogram
                statisticsChart.Series["Dice"].Points.AddXY(dice, totalRolls[dice - 1]);

                //update every 10%
                if (updateVal == (numRolls / 10))
                {
                    statisticsChart.Update();
                    updateVal = 0;
                }
                updateVal++;
            }

            //dont include index 0 and 1, since they cannot be retrieved
            totalRolls = totalRolls.SubArray(1, 11);

            float mean = sum / (numRolls);
            Sum2DiceMean.Text = mean.ToString();

            //retrieve and set minCount
            int minValue = totalRolls.Min();
            minSumCountL.Text = minValue.ToString();

            //retrieve and set minFace
            int minFace = totalRolls.ToList().IndexOf(minValue) + 2; //+1 for the offset of the subarray, +1 for of by 1
            fminSumCountL.Text = minFace.ToString();

            //retrieve and set MaxCount
            int maxValue = totalRolls.Max();
            maxSumCountL.Text = maxValue.ToString();

            //retrieve and set minFace
            int maxFace = totalRolls.ToList().IndexOf(maxValue) + 2;
            fmaxSumCountL.Text = maxFace.ToString();
        }


        //***********************DISPLAY TOOLS*******************************************
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }

        //die pic1
        private void pictureBox3_Click(object sender, EventArgs e) { }
        //die pic2
        private void pictureBox2_Click(object sender, EventArgs e) { }
        //Seed label
        private void label1_Click(object sender, EventArgs e) { }
        //seed textBox
        private void textBox1_TextChanged(object sender, EventArgs e) { }

        //sum of 2 dice label
        private void label8_Click_1(object sender, EventArgs e){}

        private void chart1_Click(object sender, EventArgs e){}

        //min count label
        private void label3_Click(object sender, EventArgs e) { }

        //static max count
        private void label4_Click(object sender, EventArgs e) { }

        //static Face Min
        private void label5_Click(object sender, EventArgs e) { }

        //static label
        private void label2_Click(object sender, EventArgs e) { }

        //static Max Face Label
        private void label6_Click(object sender, EventArgs e) { }

        //stats var label
        private void label8_Click(object sender, EventArgs e) { }
 
        //min count label var
        private void label9_Click(object sender, EventArgs e) { }

        //max count var
        private void label10_Click(object sender, EventArgs e) { }

        //min face var
        private void label11_Click(object sender, EventArgs e) { }

        //max face var
        private void label12_Click(object sender, EventArgs e) { }

        private void label13_Click(object sender, EventArgs e) { }

        //mean label
        private void label7_Click(object sender, EventArgs e) { }

        private void label16_Click(object sender, EventArgs e) { }

        private void label21_Click(object sender, EventArgs e) { }
    }
}
