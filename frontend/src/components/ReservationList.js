import React, { useState, useEffect } from 'react';
import { fetchReservations } from '../services/api';
import { Table, Button, Spinner, Alert } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';

const ReservationList = () => {
    const [reservations, setReservations] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const navigate = useNavigate();

    useEffect(() => {
        const user = JSON.parse(localStorage.getItem('user'));
        if (!user) {
            navigate('/login');
            return;
        }

        const getReservations = async () => {
            try {
                const response = await fetchReservations(user.id);

                if (Array.isArray(response)) {
                    setReservations(response);
                } else {
                    setError("Unexpected response structure.");
                }
            } catch (err) {
                setError("Error fetching reservations. Please try again.");
            } finally {
                setLoading(false);
            }
        };

        getReservations();
    }, [navigate]);

    if (loading) {
        return <Spinner animation="border" />;
    }

    if (error) {
        return <Alert variant="danger">{error}</Alert>;
    }

    return (
        <div className="container mt-4">
            <h2>My Reservations</h2>
            <Table striped bordered hover>
                <thead>
                <tr>
                    <th>Book Name</th>
                    <th>Type</th>
                    <th>Days</th>
                    <th>Quick Pickup</th>
                    <th>Total Price</th>
                </tr>
                </thead>
                <tbody>
                {reservations.length === 0 ? (
                    <tr>
                        <td colSpan="5" className="text-center">No reservations found.</td>
                    </tr>
                ) : (
                    reservations.map((reservation) => (
                        <tr key={reservation.id}>
                            <td>{reservation.bookName}</td>
                            <td>{reservation.type}</td>
                            <td>{reservation.days}</td>
                            <td>{reservation.quickPickup ? 'Yes' : 'No'}</td>
                            <td>€{(reservation.totalPrice || 0).toFixed(2)}</td>
                        </tr>
                    ))
                )}
                </tbody>
            </Table>
            <Button variant="secondary" onClick={() => navigate('/books')}>
                Go to Books
            </Button>
            <Button variant="danger" className="ms-2" onClick={() => {
                localStorage.removeItem('user');
                navigate('/login');
            }}>
                Log Out
            </Button>
        </div>
    );
};

export default ReservationList;
