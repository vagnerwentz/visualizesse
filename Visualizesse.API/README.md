# Visualizesse v1

## Transação
Uma transação conterá o usuário da transação, o valor da transação, se foi uma transação
do tipo **entrada** ou **saída**, data da criação, data da alteração, a categoria da transação
por exemplo, se foi uma transação na categoria de alimentos, lazer, saúde e etc, poderá conter a subcategoria
de uma transação na qual, o usuário poderá adicionar uma subcategoria específica.

Categoria terá criação própria pelo time de IT, e subcategorias próprias criadas pelo time de IT,
porém, usuários terão a oportunidade de criar subcategorias para o seu propósito, lembrando que
uma subcategoria criada por um usuário, não deve afetar a subcategoria criada por outro usuário.

## Balanço

Nesta versão do projeto, por enquanto, o usuário poderá apenas ter uma
wallet/balanço/carteira com o valor correspondente a somamótiro de entradas e saídas
respectivamente.

### Entrada
```
Balanço = Balanço + valor
```

### Saída
```
Balanço = Balanço - valor
```

- [ ] Um usuário, terá uma ou muitas transação.

- [ ] Este usuário, pode adicionar uma ou mais contas bancárias.
- [ ] Em cada conta bancária, o usuário poderá estar com saldos diferentes.
- [ ] Para cada transação, o usuário poderá escolher para qual conta bancária foi aquela transação, relacionando
também se foi uma entrada ou saída e o valor.
- [ ] Para a opção de débito, terá a atualização automática do valor do saldo da conta bancária selecionada.
- [ ] Para a opcão de crédito, ficará salvo o somatório dos valores das transações e ele terá a opcão de informar
que foi paga, se foi paga, haverá o débito na conta.