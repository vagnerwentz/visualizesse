import http from 'k6/http';
import { check, sleep } from 'k6';
import { randomString, randomItem } from 'https://jslib.k6.io/k6-utils/1.3.0/index.js';

export let options = {
    vus: 100, // número de usuários virtuais simultâneos
    iterations: 5000, // número total de iterações
};

export default function () {
    // Gera dados aleatórios para o usuário
    const firstName = randomString(8);
    const email = `${firstName}@example.com`;
    const password = null;

    // Dados do usuário
    const payload = JSON.stringify({
        name: firstName,
        email: email,
        password: password,
    });

    // Configurações da requisição
    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    // Envia a requisição POST para criar o usuário
    let res = http.post('http://localhost:5056/api/v1/users/register', payload, params);

    // Verifica se a requisição foi bem-sucedida
    check(res, {
        'is status 200': (r) => r.status === 200,
    });

    // Adiciona um pequeno delay entre as requisições para simular um cenário mais realista
    sleep(1);
}
