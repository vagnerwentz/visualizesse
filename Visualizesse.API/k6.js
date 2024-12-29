import http from 'k6/http';
import { check, sleep } from 'k6';
import { randomString, randomItem } from 'https://jslib.k6.io/k6-utils/1.3.0/index.js';

export let options = {
    vus: 100,
    iterations: 5000,
};

export default function () {
    const firstName = randomString(8);
    const email = `${firstName}@example.com`;
    const password = null;
    
    const payload = JSON.stringify({
        name: firstName,
        email: email,
        password: password,
    });
    
    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };
    
    let res = http.post('http://localhost:5056/api/v1/users/register', payload, params);
    
    check(res, {
        'is status 200': (r) => r.status === 200,
    });

    // Adiciona um pequeno delay entre as requisições para simular um cenário mais realista
    sleep(1);
}
