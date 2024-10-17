import React from 'react';
import { Button } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';

const Home = () => {
    const navigate = useNavigate();

    const handleLogin = () => {
        navigate('/login');
    };

    const handleRegister = () => {
        navigate('/register');
    };

    return (
        <div className="container mt-4">
            <h2>Welcome to Book Reservation System</h2>
            <p>Please login or register to continue.</p>
            <Button variant="primary" onClick={handleLogin} className="me-2">
                Login
            </Button>
            <Button variant="secondary" onClick={handleRegister}>
                Register
            </Button>
        </div>
    );
};

export default Home;
