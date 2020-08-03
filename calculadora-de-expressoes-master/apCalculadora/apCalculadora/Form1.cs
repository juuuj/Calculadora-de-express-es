using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCalculadora
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnIgual_Click(object sender, EventArgs e)
        {
            string inf = txtVisor.Text;
            string pos = paraPosfixa(inf);
            lbSequencias.Text = "Infixa: " + inf + " Pósfixa: " + pos;
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            txtVisor.Text = "";
        }

        private void novoNumero(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            txtVisor.Text += btn.Text;
        }

        private void txtVisor_TextChanged(object sender, EventArgs e)
        {
            int n;
            char[] txt = txtVisor.Text.ToCharArray();
            char ultimo = txt[txt.Length - 1];
            if (int.TryParse(ultimo.ToString(), out n) || ultimo == ')')
            {
                btnIgual.Enabled = true;
                btnSomar.Enabled = true;
                btnMultiplicar.Enabled = true;
                btnDividir.Enabled = true;
                btnElevar.Enabled = true;
                btnPonto.Enabled = true;
            }
            else
            {
                btnIgual.Enabled = false;
                btnSomar.Enabled = false;
                btnMultiplicar.Enabled = false;
                btnDividir.Enabled = false;
                btnElevar.Enabled = false;
                btnPonto.Enabled = false;
                if (ultimo == '(')
                    btnFechaParenteses.Enabled = true;
            }
            
                
        }

        private string paraPosfixa(string infixa)
        {
            string ret = "";
            char[] exp = infixa.ToCharArray();
            string ops = "()^*/+-";
            PilhaLista<char> seq = new PilhaLista<char>();
            foreach (char c in exp)
            {
                if (ops.Contains(c))
                {
                    if (seq.EstaVazia())
                        seq.Empilhar(c);
                    else
                    {
                        if (MaiorPrecedencia(seq.OTopo(), c) == seq.OTopo())
                        {
                            if (c == ')')
                            {
                                while (!seq.EstaVazia() && seq.OTopo() != '(')
                                    ret += seq.Desempilhar();
                            }
                            else
                            {
                                ret += seq.Desempilhar();
                                seq.Empilhar(c);
                            }
                        }
                        else
                            seq.Empilhar(c);
                    }
                }
                else
                    ret += c;  
            }
            return ret;
        }

        private char MaiorPrecedencia(char a, char b)
        {
            if (a == b)
                return a;
            if (a == '(' || a == ')' || b == '(' || b == ')')
                return '(';
            if (a == '^' || b == '^')
                return '^';
            if (a == '*' || a == '/')
                return a;
            if (a == '*' || a == '/')
                return b;
            return a;
        }
    }
}
