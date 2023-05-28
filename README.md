# ploomes-teste

Teste prático do projeto da ploomes, consiste em uma API RESTful cujo propósito é realizar avaliaçoes de lugares no mundo através da localização por longitude e latitude.
Também é possível cadastrar novos lugares através do CNPJ.

É possivel se cadastrar como avaliadores ou proprietários, além disso é possível obter os lugares mais próximos pela localização de um avaliador.

Para avaliar é necessário possuir um login de avaliador.

Ao avaliar é possível dar notas decimais de 0.0 a 5.0 para:

- O ambiente do lugar
- O preço do lugar
- A qualidade do serviço ou consumíveis
- O atendimento do lugar

Também é possível realizar uma avaliação anônima (O nome da pessoa não aparecerá).

Um usuário não logado pode ver os lugares e avaliações de forma limitada.



Utiliza Entity Framework Core 6,AutoMapper e o Framework Identity Core para autenticar os usuários.

### Arquitetura utilizada

 ![](Resources/Arquitetura.png)

