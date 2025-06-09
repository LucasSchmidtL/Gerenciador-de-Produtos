namespace Gerenciador_de_Produtos.Models
{
    public class DesenhoPerfil
    {
        public int PerfilId { get; set; }
        public Perfil Perfil { get; set; }

        public long DesenhoId { get; set; }
        public Desenho Desenho { get; set; }

    }
}
