namespace TronGame;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.ProgressBar barraCantidadDeCombustible;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.barraCantidadDeCombustible = new System.Windows.Forms.ProgressBar();

        //Barra de Combustible:
        this.barraCantidadDeCombustible.Location = new System.Drawing.Point(12, 12);
        this.barraCantidadDeCombustible.Name = "fuelProgressBar";
        this.barraCantidadDeCombustible.Size = new System.Drawing.Size(200, 23);
        this.barraCantidadDeCombustible.TabIndex = 0;
        this.barraCantidadDeCombustible.Maximum = 100;

        // Agregar la barra a los controles del formulario
        this.Controls.Add(this.barraCantidadDeCombustible);
        
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(900, 600); //ancho, alto
        this.Text = "Tron Game";
    }

}
