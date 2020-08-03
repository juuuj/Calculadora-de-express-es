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
        string ops = "()^*/+-"; //String com todos os operadores
        public Form1()
        {
            InitializeComponent();
            lbSequencias.Text = ""; //A label fica vazia
        }

        private void btnIgual_Click(object sender, EventArgs e)
        {
            //Reinicia as filas e pilhas, para elas não continuarem com o que estavam antes
            infixa = new FilaLista<string>();
            posfixa = new FilaLista<string>();
            pilOps = new PilhaLista<string>();

            string inf = txtVisor.Text; //Lê os números do visor
            double res = 0;
            
            Converter(inf); //Gera as sequências
            lbSequencias.Text = "Infixa: ";
            EscreverSequencia(infixa); //Escreve a sequência infixa
            lbSequencias.Text += "\nPosfixa: ";
            EscreverSequencia(posfixa); //Escreve a sequência posfixa

            try
            {
                res = Calcular(); //Calcula o resultado
                txtResultado.Text = res.ToString(); //Exibe-o no visor de resultado
            }
            catch (DivideByZeroException erro) //Se ocorrer um erro de divisão por zero
            {
                MessageBox.Show(erro.Message); //Exibe um messageBox avisando o erro
                txtResultado.Text = "Erro"; //Não é possível gerar resultado, por isso só exibe "Erro"
            }
        }
        
        private void EscreverSequencia(FilaLista<string> fil)
        {
            char letra = 'A';
            string[] vet = fil.ToArray(); //Passa a fila para vetor
            foreach (string s in vet)
            {
                if (ops.Contains(s)) //Se for um operador
                    lbSequencias.Text += s; //Exibe o operador
                else //Se for um número
                    lbSequencias.Text += letra++; //Exibe uma letra correspondente
            }
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            //Remove os textos, "zerando" a calculadora
            txtVisor.Text = "";
            txtResultado.Text = "";
            lbSequencias.Text = "";
        }

        private void novoNumero(object sender, EventArgs e)
        {
            Button btn = (Button)sender; //Identifica o botão clicado
            txtVisor.Text += btn.Text; //Escreve o valor correspondente
        }

        private void txtVisor_TextChanged(object sender, EventArgs e)
        {
            int n;
            char[] txt = txtVisor.Text.ToCharArray();
            try
            {
                char ultimo = txt[txt.Length - 1];
                if (!ops.Contains(ultimo) || ultimo == ')')  //Se o novo dígito for um número
                {
                    //Deixa habilitado os botões que podem ser apertados
                    btnIgual.Enabled = true;
                    btnSomar.Enabled = true;
                    btnMultiplicar.Enabled = true;
                    btnDividir.Enabled = true;
                    btnElevar.Enabled = true;
                    btnPonto.Enabled = true;
                }
                else //Se o novo dígito não for um número
                {
                    //Deixa habilitado os botões que não podem ser apertados
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

        private void Converter (string inf)
        {
            string elemento = "";
            string op = "";

            for (int i=0; i<inf.Length; i++) //Passa por todos os itens da sequencia infixa
            {
                elemento = "";
                if (!ops.Contains(inf[i])) //Se o item lido não for um operador
                {
                    elemento = "";
                    int inicial = i; //index do início do numero
                    while (inicial + elemento.Length < inf.Length && (!ops.Contains(inf[inicial + elemento.Length].ToString()) || inf[inicial + elemento.Length] == '.')) //Enquanto o elemento ainda for um número
                        elemento += inf[inicial + elemento.Length]; //O número é incrementado                
                    i = inicial + elemento.Length - 1; //o for continua a partir de onde a leitura parou
                    posfixa.Enfileirar(elemento); //O número é adicionado na fila posfixa
                }
                else //Se o item lido for um operador
                {
                    elemento = inf[i].ToString();  //O elemento recebe o item lido
                    while (!pilOps.EstaVazia() && Precedencia(pilOps.OTopo()) >= Precedencia(elemento[0].ToString()))//Enquanto o primeiro operador da pilha tiver precedência sobre os outros
                    {
                        op = pilOps.OTopo(); //Identifica o operador a ser tratado
                        if (op == "(")
                            break;
                        posfixa.Enfileirar(op.ToString());  //O operador é adicionado na sequencia posfixa
                        pilOps.Desempilhar(); //E removido da pilha
                    }
                    if (elemento == ")") //Se o operador for o fecha parênteses
                        pilOps.Desempilhar(); //Desempilha o ultimo operador
                    else //Se não
                        pilOps.Empilhar(elemento); //Empilha o operador normalmente
                }
                if (elemento != "(" && elemento != ")") //Verifica se o item não é ( nem ), pois esses não entram na sequência
                    infixa.Enfileirar(elemento); //Adiciona o item na infixa
            }
            while (!pilOps.EstaVazia()) //Até a pilha ficar vazia
            {
                op = pilOps.Desempilhar(); //Desempilha o operador
                if (op != "(" && op != ")") //Verifica se o item não é ( nem ), pois esses não entram na sequência
                    posfixa.Enfileirar(op); //Enfilera o operador na posfixa
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
                            ret = b * a;
                            break;
                        case "/":
                            if (a == 0)
                                throw new DivideByZeroException("Divisão por 0");
                            ret = b / a;
                            break;
                        case "+":
                            ret = b + a;
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
            if (o == "(")
                return 2;
            if (o == "*" || o == "/")
                return 1;
            if (o == "+" || o == "-")
                return 0;
            return -1;
        }

        private void btnVolta_Click(object sender, EventArgs e)
        {
            if (txtVisor.Text == "")
                return;
            try
            {
                int tamanho = txtVisor.Text.Length;
                string txt = txtVisor.Text.Remove(tamanho-1, 1);
                txtVisor.Text = txt;
                txtResultado.Text = "";
                lbSequencias.Text = "";
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
        }
    }
}
