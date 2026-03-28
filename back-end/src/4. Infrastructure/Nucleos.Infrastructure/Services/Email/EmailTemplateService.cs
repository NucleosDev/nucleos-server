namespace Nucleos.Infrastructure.Services.Email;

public class EmailTemplateService
{
    public string BoasVindas(string nome) =>
        $"<h1>Bem-vindo ao Nucleos, {nome}!</h1><p>Comece a organizar sua vida agora.</p>";

    public string ResetSenha(string token) =>
        $"<p>Clique no link para resetar sua senha: <a href='https://app.nucleos.io/reset?token={token}'>Resetar senha</a></p>";
}
