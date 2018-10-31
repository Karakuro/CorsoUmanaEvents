using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lezione12.Events
{
    public partial class Form1 : Form
    {
        //Delegates necessari per far passare l'esecuzione da un Thread all'altro
        //Maggiori informazioni nei metodi ThreadUpdate() e ThreadDone()
        public delegate void MyDelegate();
        public delegate void MyStatusDelegate(params int[] progress);

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Metodo che reagisce al click sul bottone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            int total = (int)this.numericUpDown1.Value;
            if (total > 0)
            {
                //Inizializzazione della classe che opererà nel task parallelo
                Engine eng = new Engine(total);

                //Sottoscrizione dell'evento di aggiornamento
                eng.OnUpdateStatus += ThreadUpdate;

                //Sottoscrizione dell'evento di completamento
                eng.OnComplete += ThreadDone;

                //Avvio del Thread sul metodo dell'oggetto eng
                Task.Run(() => {
                    eng.Start();
                });

            }
        }

        private void ThreadUpdate(object sender, ProgressEventArgs e)
        {
            /*
             * NON visto a lezione, provare a capirlo:
             * in questo punto l'esecuzione è ancora in mano al Task secondario
             * chiamato alla fine del metodo button1_click, il quale NON ha accesso
             * alle componenti della form, che sono gestite dal Task primario.
             * Quindi, per poter comunicare al Task primario le informazioni necessarie
             * per l'aggiornamento dei suoi componenti con il nuovo valore di progress,
             * si deve usufruire del metodo Control.Invoke, che, come si può leggere dalla definizione
             * che riporto in fondo a questa classe, si occupa di chiamare il metodo fornito al
             * delegate 'd' con gli eventuali parametri e di assegnarne il controllo al Task Primario, 
             * rappresentato in questo punto dalla parola chiave 'this'.
             * Verificate infatti che, posizionando il mouse sopra la parola chiave 'this', vi viene
             * segnalato che corrisponde a un oggetto della classe Form1, ovvero quello che sta gestendo
             * in questo momento la finestra della Form.
            */
            MyStatusDelegate d = UpdateState;
            this.Invoke(d, new int[] { e.Progress });
        }
        public void ThreadDone(object sender, EventArgs e)
        {
            /*
             * NON visto a lezione, provare a capirlo:
             * in questo punto l'esecuzione è ancora in mano al Task secondario
             * chiamato alla fine del metodo button1_click, il quale NON ha accesso
             * alle componenti della form, che sono gestite dal Task primario.
             * Quindi, per poter comunicare al Task primario le informazioni necessarie
             * per l'aggiornamento dei suoi componenti con il nuovo valore di progress,
             * si deve usufruire del metodo Control.Invoke, che, come si può leggere dalla definizione
             * che riporto in fondo a questa classe, si occupa di chiamare il metodo fornito al
             * delegate 'd' con gli eventuali parametri e di assegnarne il controllo al Task Primario, 
             * rappresentato in questo punto dalla parola chiave 'this'.
             * Verificate infatti che, posizionando il mouse sopra la parola chiave 'this', vi viene
             * segnalato che corrisponde a un oggetto della classe Form1, ovvero quello che sta gestendo
             * in questo momento la finestra della Form.
            */
            MyDelegate del = new MyDelegate(EndExecution);
            this.Invoke(del);
        }

        /// <summary>
        /// Metodo compatibile col delegate MyStatusDelegate che si occupa di aggiornare la barra di avanzamento con il valore aggiornato
        /// </summary>
        /// <param name="progress"></param>
        private void UpdateState(params int[] progress)
        {
            this.progressBar1.Value = progress[0];
            this.label1.Text = string.Format("{0}%", progress[0]);
        }

        /// <summary>
        /// Metodo compatibile col delegate MyDelegate che si occupa di gestire il completamento dell'esecuzione del Task
        /// </summary>
        public void EndExecution()
        {
            MessageBox.Show("Esecuzione terminata. Ora prova a cliccare più volte 'Start'");
            this.progressBar1.Value = 0;
            this.label1.Text = "0%";
        }
    }
}


/*
    // Summary:
    //     Executes the specified delegate, on the thread that owns the control's underlying
    //     window handle, with the specified list of arguments.
    //
    // Parameters:
    //   method:
    //     A delegate to a method that takes parameters of the same number and type that
    //     are contained in the args parameter.
    //
    //   args:
    //     An array of objects to pass as arguments to the specified method. This parameter
    //     can be null if the method takes no arguments.
    //
    // Returns:
    //     An System.Object that contains the return value from the delegate being invoked,
    //     or null if the delegate has no return value.
    public object Invoke(Delegate method, params object[] args);
*/
