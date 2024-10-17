import React, { useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import { createReservation } from '../services/api';

const BookModal = ({ show, handleClose, book }) => {
    const [type, setType] = useState('Book');
    const [days, setDays] = useState(1);
    const [quickPickup, setQuickPickup] = useState(false);

    const handleReservation = () => {
        const user = JSON.parse(localStorage.getItem('user'));
        if (!user || !user.id) {
            alert('User is not logged in!');
            return;
        }

        const reservationData = {
            bookId: book.id,
            type: type,
            days: days,
            quickPickup: quickPickup,
            userId: user.id,
            bookName: book.name
        };

        createReservation(reservationData)
            .then(() => {
                alert('Reservation successful!');
                handleClose();
            })
            .catch((error) => {
                alert('Failed to reserve the book. Please try again.');
            });
    };

    return (
        <Modal show={show} onHide={handleClose}>
            <Modal.Header closeButton>
                <Modal.Title>Reserve {book.name}</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group controlId="reservationType">
                        <Form.Label>Select Type</Form.Label>
                        <Form.Control
                            as="select"
                            value={type}
                            onChange={(e) => setType(e.target.value)}
                        >
                            <option value="Book">Book (€2/day)</option>
                            <option value="Audiobook">Audiobook (€3/day)</option>
                        </Form.Control>
                    </Form.Group>

                    <Form.Group controlId="reservationDays" className="mt-3">
                        <Form.Label>For how many days</Form.Label>
                        <Form.Control
                            type="number"
                            value={days}
                            min={1}
                            onChange={(e) => setDays(Number(e.target.value))}
                        />
                    </Form.Group>

                    <Form.Group controlId="quickPickup" className="mt-3">
                        <Form.Check
                            type="checkbox"
                            label="Quick Pick-up (€5)"
                            checked={quickPickup}
                            onChange={(e) => setQuickPickup(e.target.checked)}
                        />
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>
                    Close
                </Button>
                <Button variant="primary" onClick={handleReservation}>
                    Reserve
                </Button>
            </Modal.Footer>
        </Modal>
    );
};

export default BookModal;
