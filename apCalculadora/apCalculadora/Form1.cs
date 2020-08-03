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
        FilaLista<string> infixa = new FilaLista<string>();
        FilaLista<string> posfixa = new FilaLista<string>();
        PilhaLista<string> pilOps = new PilhaLista<string>();
        string ops = "()^*/+-";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnIgual_Click(object sender, EventArgs e)
        {
            //FilaLista<string> infixa = new FilaLista<string>();
            //FilaLista<string> posfixa = new FilaLista<string>();
            //PilhaLista<string> pilOps = new PilhaLista<string>();
                
            string inf = txtVisor.Text;
            //string pos;
            //string[] valores = ParaVetorNumerico(inf);
            //string infComLetras = NumerosParaLetras(inf);
            
            Converter(inf);
            lbSequencias.Text = "Infixa: ";
            EscreverSequencia(infixa);
            lbSequencias.Text = "\nPosfixa: ";
            EscreverSequencia(posfixa);

            double res = Calcular();
            txtResultado.Text = res.ToString();
        }
        
        private void EscreverSequencia(FilaLista<string> fil)
        {
            char letra = 'A';
            string[] vet = fil.ToArray();
            foreach (string s in vet)
            {
                if (ops.Contains(s))
                    lbSequencias.Text += s;
                else
                    lbSequencias.Text += letra++;
            }
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
            try
            {
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
            } catch (Exception erro) { }    
        }

        private string[] ParaVetorNumerico(string seq)
        {
            int n;
            string[] ret = new string[seq.Length];
            string atual = "";
            int i = 0;
            foreach (char c in seq.ToCharArray())
            {
                if (int.TryParse(c.ToString(), out n))
                {
                    atual += n.ToString();
                }
                else if (c == '.')
                {
                    atual += ".";
                }
                else
                {
                    ret[i] = atual;
                    i++;
                    atual = "";
                }
            }
            ret[i] = atual;
            return ret;
        }

        private string NumerosParaLetras(string seq)
        {
            string alfabeto = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] letras = alfabeto.ToCharArray();

            int n;
            string ret = "";
            string atual = "";
            int i = 0;
            foreach (char c in seq.ToCharArray())
            {
                if (int.TryParse(c.ToString(), out n))
                {
                    atual += n.ToString();
                }
                else if (c == '.')
                {
                    atual += ".";
                }
                else
                {
                    ret += letras[i] + c.ToString();
                    i++;
                    atual = "";
                }
            }
            ret += letras[i];
            //MessageBox.Show(ret);
            return ret;
        }

        //private string ParaPosfixa(string infixa)
        //{
        //    string ret = "";
        //    char[] exp = infixa.ToCharArray();
        //    string ops = "()^*/+-";
        //    PilhaLista<char> seq = new PilhaLista<char>();
        //    //foreach (char c in exp)
        //    //{
        //    //    if (ops.Contains(c))
        //    //    {
        //    //        if (seq.EstaVazia())
        //    //            seq.Empilhar(c);
        //    //        else
        //    //        {
        //    //            if (MaiorPrecedencia(seq.OTopo(), c) == seq.OTopo())
        //    //            {
        //    //                if (c == ')')
        //    //                {
        //    //                    while (!seq.EstaVazia() && seq.OTopo() != '(')
        //    //                        ret += seq.Desempilhar();
        //    //                }
        //    //                else
        //    //                {
        //    //                    ret += seq.Desempilhar();
        //    //                    seq.Empilhar(c);
        //    //                }
        //    //            }
        //    //            else
        //    //                seq.Empilhar(c);
        //    //        }
        //    //    }
        //    //    else
        //    //        ret += c;  
        //    //}
        //    for (int i=0; i<infixa.Length; i++)
        //    {
        //        char c = infixa[i]; 
        //        if (ops.Contains(c))
        //        {
        //            if (seq.EstaVazia())
        //            {
        //                seq.Empilhar(c);
        //            }
        //            else
        //            {
        //                if (MaiorPrecedencia(seq.OTopo(), c) == seq.OTopo())
        //                {
        //                    if (c == '+' || c == '-')
        //                    {
        //                        ret += seq.Desempilhar();
        //                        i--;
        //                    }
        //                    else
        //                    {
        //                        ret += seq.Desempilhar();
        //                        i--;
        //                    }
        //                }
        //                else
        //                {
        //                    if (c == '+' || c == '-')
        //                    {
        //                        ret += seq.Desempilhar();
        //                        seq.Empilhar(c);
        //                    }
        //                    else
        //                    {
        //                        seq.Empilhar(c);
        //                    }
        //                }   
        //            }
        //        }
        //        else
        //        {
        //            ret += c;
        //        }
        //    }
        //    return ret;
        //}

        //public string ParaPosfixa(string inf)
        //{
        //    string ret = "";
        //    char[] exp = inf.ToCharArray();
        //    string ops = "()^*/+-";
        //    PilhaLista<string> pilOps = new PilhaLista<string>();

        //    for (int i = 0; i < inf.Length; i++)
        //    {
        //        string elemento = "";

        //        if (!ops.Contains(inf[i].ToString()))
        //        {
        //            elemento = "";
        //            int inicial = i;
        //            while (inicial + elemento.Length < inf.Length && (!ops.Contains(inf[inicial + elemento.Length].ToString()) || inf[inicial + elemento.Length] == '.'))
        //                elemento += inf[inicial + elemento.Length];
        //            i = inicial + elemento.Length - 1;
        //            //posfixa.Enfileirar(elemento);
        //        }
        //        else
        //        {
        //            if (inf[i] == '+' && (i == 0 || inf[i - 1] == '('))
        //                elemento = "#";
        //            else
        //            if (inf[i] == '-' && (i == 0 || inf[i - 1] == '('))
        //                elemento = "@";
        //            else
        //                elemento = inf[i] + "";

        //            while (!pilOps.EstaVazia() && Precedencia(pilOps.OTopo()[0] )> Precedencia (elemento[0]))
        //            {
        //                string op = pilOps.Desempilhar();
        //                /*if (op != "(" && op != ")")
        //                    posfixa.Enfileirar(op);*/
        //            }
        //            if (elemento != ")")
        //                pilOps.Empilhar(elemento);
        //        }
        //        if (elemento != "(" && elemento != ")")
        //            ret += elemento;
        //            //infixa.Enfileirar(elemento);
        //    }

        //    //TratarPilhaOperadores();
        //    while (!pilOps.EstaVazia())
        //    {
        //        string o = pilOps.Desempilhar();
        //        ret += o;
        //    }
        //    //sequenciaInfixa = EscreverSequencia(infixa);
        //    //sequenciaPosfixa = EscreverSequencia(posfixa);
        //    //resultado = CalcularResultado();
        //    //return resultado;
        //    return ret;
        //}

        private void Converter (string inf)
        {
            for (int i=0; i<inf.Length; i++)
            {
                string elemento = "";
                if (ops.Contains(inf[i]))
                {
                    elemento = "";
                    int inicial = i;
                    while (inicial + elemento.Length < inf.Length && (!ops.Contains(inf[inicial + elemento.Length].ToString()) || inf[inicial + elemento.Length] == '.'))
                        elemento += inf[inicial + elemento.Length];                  
                    i = inicial + elemento.Length - 1;
                    posfixa.Enfileirar(elemento);
                }
                else
                {
                    elemento = inf[i].ToString();
                    while (!pilOps.EstaVazia() && Precedencia(pilOps.OTopo()) > Precedencia(elemento[0].ToString()))
                    {
                        char op = pilOps.OTopo()[0];
                        if (op == '(')
                            break;
                        posfixa.Enfileirar(op.ToString());
                        pilOps.Desempilhar();
                    }
                    if (elemento != ")")
                        pilOps.Empilhar(elemento);
                    else
                        pilOps.Desempilhar();
                }
                if (elemento != "(" && elemento != ")")
                    infixa.Enfileirar(elemento);
            }
            while (!pilOps.EstaVazia())
            {
                string op = pilOps.Desempilhar();
                if (op != "(" && op != ")")
                    posfixa.Enfileirar(op);
            }
        }

        private double Calcular()
        {
            PilhaLista<double> valores = new PilhaLista<double>();
            double a=0, b=0, ret=0;
            string[] vet = posfixa.ToArray();

            foreach (string s in vet)
            {
                if (!ops.Contains(s))
                    valores.Empilhar(double.Parse(s.Replace('.', ',')));
                else
                {
                    a = valores.Desempilhar();
                    b = valores.Desempilhar();
                    switch(s)
                    {
                        case "^":
                            ret = Math.Pow(b, a);
                            break;
                        case "*":
                            ret = a * b;
                            break;
                        case "/":
                            if (a == 0)
                                throw new DivideByZeroException("Divisão por 0");
                            ret = b / a;
                            break;
                        case "+":
                            ret = a + b;
                            break;
                        case "-":
                            ret = b - a;
                            break;
                        default:
                            break;
                    }
                    valores.Empilhar(ret);
                }
            }
            return valores.Desempilhar();
        }

        private int Precedencia(string o)
        {
            if (o == "(" || o == ")")
                return 2;
            if (o == "*" || o == "/")
                return 1;
            if (o == "+" || o == "-")
                return 0;
            return -1;
        }

        private void btnVolta_Click(object sender, EventArgs e)
        {
            try
            {
                int tamanho = txtVisor.Text.Length;
                string txt = txtVisor.Text.Remove(tamanho-1, 1);
                txtVisor.Text = txt;
            }catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
        }
    }
}
