# Documento de Engenharia de Software (em construção): Visualizesse

## Introdução
O Visualizesse é um sistema de gerenciamento financeiro que permite aos usuários registrar suas transações financeiras, controlar seus gastos e analisar seus hábitos de consumo. O sistema é projetado para ajudar os usuários a melhorar sua saúde financeira, fornecendo ferramentas para monitorar suas despesas e receitas.

Este documento descreve os casos de uso, requisitos funcionais e não funcionais do Visualizesse, delineando as principais funcionalidades e características do sistema.

## Casos de Uso
### UC-01: Registrar Usuário
**Descrição:** Este caso de uso permite que um usuário se registre na plataforma fornecendo seu nome, email e senha.

### UC-02: Criar Wallet
**Descrição:** Um usuário pode criar uma carteira com um saldo inicial de 0 e uma descrição para identificação.

### UC-03: Criar Subcategoria
**Descrição:** Um usuário pode criar uma subcategoria associada a uma categoria existente, fornecendo o ID da categoria e uma descrição para a subcategoria.

### UC-04: Deletar Transação
**Descrição:** Um usuário pode excluir uma transação financeira registrada no sistema.

### UC-05: Editar Transação
**Descrição:** Um usuário pode editar os detalhes de uma transação financeira existente, como valor, descrição, categoria, etc.

### UC-06: Listar Transações
**Descrição:** Um usuário pode visualizar uma lista de todas as suas transações financeiras registradas no sistema.

## Requisitos Funcionais
1. O sistema deve permitir que os usuários se registrem na plataforma fornecendo nome, email e senha.
2. O sistema deve permitir que os usuários criem carteiras para controlar seus saldos financeiros.
3. O sistema deve permitir que os usuários criem subcategorias associadas a categorias existentes.
4. O sistema deve permitir que os usuários excluam transações financeiras registradas.
5. O sistema deve permitir que os usuários editem os detalhes de transações financeiras existentes.
6. O sistema deve permitir que os usuários visualizem uma lista de todas as suas transações financeiras.

## Requisitos Não Funcionais
1. **Segurança:** O sistema deve garantir a segurança dos dados dos usuários, protegendo as informações de acesso e transações financeiras.
2. **Desempenho:** O sistema deve ser responsivo e ter tempos de resposta rápidos, mesmo sob carga pesada.
3. **Usabilidade:** A interface do usuário deve ser intuitiva e fácil de usar, garantindo uma experiência agradável para o usuário.
4. **Disponibilidade:** O sistema deve estar disponível 24/7, garantindo que os usuários possam acessá-lo a qualquer momento.
5. **Compatibilidade:** O sistema deve ser compatível com uma variedade de dispositivos e navegadores para garantir acessibilidade universal.
6. **Manutenibilidade:** O código-fonte do sistema deve ser bem estruturado e documentado, facilitando a manutenção e a evolução do sistema ao longo do tempo.

## Requisitos diferenciais
1. **Rollback transaction**: O sistema deve garantir que uma transação só poderá alterar o balanço na carteira, se dado a request inteira de manuseio da transação e da carteira estiverem sido realizadas com sucesso.

## Conclusão
O Visualizesse é uma ferramenta poderosa para ajudar os usuários a controlar suas finanças e melhorar sua saúde financeira. Com uma variedade de funcionalidades e 
uma abordagem centrada no usuário, o sistema oferece uma solução abrangente para gerenciar transações financeiras e analisar padrões de gastos. 
Ao aderir aos requisitos funcionais e não funcionais delineados neste documento, o Visualizesse está preparado para fornecer uma experiência confiável e eficaz para seus usuários.
