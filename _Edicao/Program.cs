using System;
using System.Windows.Forms;

namespace Modus
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Inicializa o formulário principal e passa o argumento
            Form1 mainForm = new Form1();

            // Se o programa foi aberto com um arquivo arrastado
            if (args.Length > 0)
            {
                string filePath = args[0];
                mainForm.ProcessFile(filePath); // Chama o método do Form1
            }

            Application.Run(mainForm);
        }
    }
}
