import React, { useState, useEffect } from 'react';
import { Form, Button, Alert, Spinner } from 'react-bootstrap';
import { loginUser } from '../services/api';
import { useNavigate } from 'react-router-dom';

const Login = () => {
    const [name, setName] = useState('');
    const [password, setPassword] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        const user = JSON.parse(localStorage.getItem('user'));
        if (user) {
            navigate('/books');
        }
    }, [navigate]);

    const handleLogin = async (credentials) => {
        try {
            setLoading(true);
            const response = await loginUser(credentials);
            localStorage.setItem('user', JSON.stringify({ id: response.userId, name: response.name }));
            navigate('/books');
        } catch (error) {
            setError('Login failed: ' + (error.message || error));
        } finally {
            setLoading(false);
        }
    };

    const handleBackToMainMenu = () => {
        navigate('/');
    };

    return (
        <div className="container mt-4">
            <h2>Login</h2>
            <Form onSubmit={(e) => { e.preventDefault(); handleLogin({ name, password }); }}>
                {error && <Alert variant="danger">{error}</Alert>}

                <Form.Group controlId="name">
                    <Form.Label>Name</Form.Label>
                    <Form.Control
                        type="text"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        placeholder="Enter your name"
                        required
                    />
                </Form.Group>

                <Form.Group controlId="password" className="mt-3">
                    <Form.Label>Password</Form.Label>
                    <Form.Control
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        placeholder="Enter your password"
                        required
                    />
                </Form.Group>

                <Button className="mt-3" variant="primary" type="submit" disabled={loading}>
                    {loading ? <Spinner as="span" animation="border" size="sm" /> : 'Login'}
                </Button>

                <Button className="mt-3 ms-2" variant="secondary" onClick={handleBackToMainMenu}>
                    Back to Main Menu
                </Button>
            </Form>
        </div>
    );
};

export default Login;
