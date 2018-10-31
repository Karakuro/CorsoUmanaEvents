using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lezione12.Events
{
    public class Engine
    {
        /// <summary>
        /// Evento che permette a qualsiasi altro Task di ricevere aggiornamenti sullo status di questo Task
        /// </summary>
        public event EventHandler<ProgressEventArgs> OnUpdateStatus = delegate { };
        /// <summary>
        /// Evento che permette a qualsiasi altro Task di ricevere aggiornamenti sul completamento di questo Task
        /// </summary>
        public event EventHandler OnComplete = delegate { };

        //Variabili private
        private int _counter;
        private int _total;

        /// <summary>
        /// Costruttore che prende in ingresso il totale di cicli di sleep da eseguire
        /// </summary>
        /// <param name="Total">Il totale di cicli di sleep da eseguire</param>
        public Engine(int Total)
        {
            _counter = 0;
            _total = Total;
        }

        /// <summary>
        /// Serve unicamente a far fare qualcosa al Thread
        /// in modo da non farlo terminare istantaneamente
        /// </summary>
        public void Start()
        {
            for(_counter = 0; _counter <= _total; _counter++)
            {
                UpdateStatus();
                Thread.Sleep(10);
            }
            OnComplete(this, EventArgs.Empty);
        }

        /// <summary>
        /// Metodo che viene chiamato a ogni ciclo per inviare a chiunque sia in ascolto l'aggiornamento di stato
        /// </summary>
        private void UpdateStatus()
        {
            //Traduco il rapporto tra il contatore e il totale in un valore %
            int progress = (_counter * 100) / _total;
            //Creo l'oggetto che eredita da EventArgs per assegnargli lo stato di avanzamento in %
            ProgressEventArgs args = new ProgressEventArgs(progress);
            //Lancio l'evento della classe Engine con this = l'istanza attualmente attiva di tipo Engine 
            //e l'oggetto args che contiene i valori da trasmettere a chiunque si sia registrato all'evento
            OnUpdateStatus(this, args);
        }
    }

    public class ProgressEventArgs : EventArgs
    {
        public int Progress { get; private set; }

        public ProgressEventArgs(int progress)
        {
            Progress = progress;
        }
    }
}
