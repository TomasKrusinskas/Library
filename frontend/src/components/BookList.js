import React, { useState, useEffect } from 'react';
import { fetchBooks } from '../services/api';
import { Card, Button, Spinner, Alert, Form, InputGroup } from 'react-bootstrap';
import BookModal from './BookModal';
import { useNavigate } from 'react-router-dom';

const useDebounce = (value, delay) => {
    const [debouncedValue, setDebouncedValue] = useState(value);

    useEffect(() => {
        const handler = setTimeout(() => {
            setDebouncedValue(value);
        }, delay);

        return () => {
            clearTimeout(handler);
        };
    }, [value, delay]);

    return debouncedValue;
};

const BookList = () => {
    const [books, setBooks] = useState([]);
    const [selectedBook, setSelectedBook] = useState(null);
    const [showModal, setShowModal] = useState(false);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const [searchTerm, setSearchTerm] = useState('');
    const [searchYear, setSearchYear] = useState('');
    const [searchType, setSearchType] = useState('');

    const navigate = useNavigate();

    const debouncedSearchTerm = useDebounce(searchTerm, 300);
    const debouncedSearchYear = useDebounce(searchYear, 300);
    const debouncedSearchType = useDebounce(searchType, 300);

    useEffect(() => {
        const getBooks = async () => {
            setLoading(true);
            setError(null);
            try {
                const response = await fetchBooks({
                    name: debouncedSearchTerm,
                    year: debouncedSearchYear,
                    type: debouncedSearchType,
                });
                setBooks(response);
            } catch (err) {
                setError("Error fetching books. Please try again later.");
            } finally {
                setLoading(false);
            }
        };

        getBooks();
    }, [debouncedSearchTerm, debouncedSearchYear, debouncedSearchType]);

    const handleBookClick = (book) => {
        setSelectedBook(book);
        setShowModal(true);
    };

    const handleCloseModal = () => {
        setShowModal(false);
        setSelectedBook(null);
    };

    const handleGoToReservations = () => {
        navigate('/reservations');
    };

    if (loading) {
        return <Spinner animation="border" className="mt-4" />;
    }

    if (error) {
        return <Alert variant="danger" className="mt-4">{error}</Alert>;
    }

    return (
        <div className="container mt-4">
            <h2>Books List</h2>
            <InputGroup className="mb-3">
                <Form.Control
                    type="text"
                    placeholder="Search by name"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                />
                <Form.Control
                    type="number"
                    placeholder="Year"
                    value={searchYear}
                    onChange={(e) => setSearchYear(e.target.value)}
                />
                <Form.Select
                    value={searchType}
                    onChange={(e) => setSearchType(e.target.value)}
                >
                    <option value="">All Types</option>
                    <option value="Book">Book</option>
                    <option value="Audiobook">Audiobook</option>
                </Form.Select>
            </InputGroup>
            <div className="row">
                {books.length > 0 ? (
                    books.map((book) => (
                        <div key={book.id} className="col-md-4 mb-3">
                            <Card>
                                <Card.Img
                                    variant="top"
                                    src={book.pictureUrl}
                                    alt={book.name}
                                    style={{ maxWidth: '100%', maxHeight: '200px', objectFit: 'cover' }}
                                />
                                <Card.Body>
                                    <Card.Title>{book.name}</Card.Title>
                                    <Card.Text>Year: {book.year}</Card.Text>
                                    <Card.Text>Type: {book.type}</Card.Text>
                                    <Button variant="primary" onClick={() => handleBookClick(book)}>
                                        Reserve
                                    </Button>
                                </Card.Body>
                            </Card>
                        </div>
                    ))
                ) : (
                    <p>No books available</p>
                )}
            </div>
            {selectedBook && (
                <BookModal
                    show={showModal}
                    handleClose={handleCloseModal}
                    book={selectedBook}
                />
            )}
            <Button variant="secondary" onClick={handleGoToReservations}>
                Go to Reservations
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

export default BookList;
