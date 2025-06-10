# ✅ Resumo prático - Autenticação JWT no Backend

| Item                         | Significado                                                                 |
|------------------------------|-----------------------------------------------------------------------------|
| `AddAuthentication("Bearer")` | Define o esquema padrão esperado para tokens na API                         |
| `AddJwtBearer(...)`          | Configura a validação do token JWT emitido pelo IdentityServer              |
| `Authority`                  | Endereço do IdentityServer (quem emitiu o token)                            |
| `ValidateAudience = false`   | Ignora a validação do campo `aud` do token                                  |
| `AddPolicy("ApiScop", ...)`  | Cria uma política de acesso com base no escopo do token                     |
| `[Authorize(Policy = "...")]`| Exige que o token JWT contenha os dados esperados para acessar o endpoint   |

---

# ✅ Resumo prático da configuração Web (Cliente)

| Item                     | Significado                                                                 |
|--------------------------|-----------------------------------------------------------------------------|
| `AddAuthentication(...)`| Define o esquema de autenticação padrão. Neste caso:                         |
| `DefaultScheme = "Cookies"`        | Autenticação via cookies (usado para manter o usuário logado no navegador). |
| `DefaultChallengeScheme = "oidc"`  | Quando a autenticação for exigida, o desafio será via OpenID Connect.      |

---

## 🍪 `AddCookie("Cookies", ...)`

| Item                              | Significado                                                            |
|-----------------------------------|------------------------------------------------------------------------|
| `ExpireTimeSpan = TimeSpan.FromMinutes(10)` | Define que o cookie de login vai expirar após 10 minutos de inatividade. |
| *(Reautenticação ocorrerá via OIDC após expiração)* |                                                                    |

---

## 🔐 `AddOpenIdConnect("oidc", ...)`

| Item                                | Significado                                                                 |
|-------------------------------------|-----------------------------------------------------------------------------|
| `Authority`                         | URL do IdentityServer (quem fornece a autenticação).                       |
| `GetClaimsFromUserInfoEndpoint = true` | Após o login, busca claims extras no endpoint de `userinfo`.            |
| `ClientId`                          | Identifica esta aplicação cliente no IdentityServer.                       |
| `ClientSecret`                      | Segredo compartilhado usado na autenticação do cliente.                    |
| `ResponseType = "code"`             | Usa o Authorization Code Flow (fluxo mais seguro, com token via backend). |
| `ClaimActions.MapJsonKey("role", "role", "role")` | Mapeia a claim `role` do JSON para a claim de identidade do .NET. |
| `ClaimActions.MapJsonKey("sub", "sub", "sub")`   | Mapeia a claim `sub` (subject, ou ID do usuário).               |
| `TokenValidationParameters.NameClaimType = "name"` | Define qual claim será usada como `User.Identity.Name`.         |
| `TokenValidationParameters.RoleClaimType = "role"` | Define qual claim será usada como `User.IsInRole(...)`.         |
| `Scope.Add("geek_shopping")`        | Pede acesso ao escopo `geek_shopping`, necessário para acesso às APIs.     |
| `SaveTokens = true`                 | Salva os tokens de acesso e refresh no cookie para uso posterior.          |

---

## 📘 Exemplo de uso

Quando o usuário tentar acessar uma rota protegida:

1. Será redirecionado para o **IdentityServer**.
2. Após o login, o IdentityServer devolverá um **código de autorização**.
3. O código será trocado por um **access token + ID token**.
4. O token será salvo no **cookie** da aplicação.
5. O usuário estará autenticado com suas **claims** (nome, role, etc.) disponíveis.
