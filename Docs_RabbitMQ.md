# Resumo da Configuração do RabbitMQMessageSender

## Fluxo básico do envio de mensagem

1. Criar uma conexão com o RabbitMQ  
2. Criar um canal dentro da conexão  
3. Declarar a fila (criar se não existir)  
4. Serializar a mensagem para JSON e transformar em array de bytes  
5. Publicar a mensagem na fila  
6. Fechar o canal automaticamente com `using`  

---

## Detalhes importantes

- **Conexão (`IConnection`)**  
  - Criada toda vez que o método `SendMessageAsync` é chamado (no seu código atual).  
  - Conexão TCP física e custosa para abrir.  
  - Recomenda-se criar uma única conexão (Singleton) e reutilizá-la.

- **Canal (`IModel` ou `IChannel`)**  
  - Criado a cada envio, dentro do método, via `CreateChannelAsync()`.  
  - Canal é leve e não thread-safe, deve ser criado por envio/thread.  
  - Fechado automaticamente ao final do método por causa do `using`.

- **Fila (`QueueDeclareAsync`)**  
  - Declarada toda vez antes do envio para garantir que existe.  
  - No seu código está com `durable: false`, ou seja, não persiste após restart do RabbitMQ.

---


## Parâmetros da declaração da fila (`QueueDeclareAsync`)

```csharp
await channel.QueueDeclareAsync(
    queue: queueName, 
    durable: false, 
    exclusive: false, 
    autoDelete: false, 
    arguments: null);


| Parâmetro    | Tipo                          | Descrição                                                                                             |
| ------------ | ----------------------------- | ----------------------------------------------------------------------------------------------------- |
| `queue`      | `string`                      | Nome da fila.                                                                                         |
| `durable`    | `bool`                        | Se `true`, a fila é persistente e sobrevive a reinícios do RabbitMQ. Se `false`, a fila é temporária. |
| `exclusive`  | `bool`                        | Se `true`, a fila só pode ser usada pela conexão que a criou e será deletada ao fechar essa conexão.  |
| `autoDelete` | `bool`                        | Se `true`, a fila será deletada automaticamente quando não houver mais consumidores conectados.       |
| `arguments`  | `IDictionary<string, object>` | Parâmetros opcionais para configurações avançadas, como TTL, dead-letter exchanges, etc.              |

```

## Boas práticas recomendadas

- Criar a conexão uma vez (Singleton) e reutilizá-la para todos os envios.
- Criar um canal por envio e fechá-lo após o uso (com `using`).
- Declarar a fila com `durable: true` para persistência.
- Marcar mensagens como persistentes para evitar perda após reinício.
- Utilizar `await` para manter operações assíncronas e não bloquear threads.

