import './App.css';
import React from 'react';
import ReactDOM from 'react-dom';
import {Elements} from '@stripe/react-stripe-js';
import {loadStripe} from '@stripe/stripe-js';

import CheckoutForm from './checkout-form/index.jsx';

const stripePromise = loadStripe("pk_test_bxJuE0fpBxauHmThIvNnWtDt");

function App() {
  return (
    <Elements stripe={stripePromise}>
      <CheckoutForm />
    </Elements>
  );
}

ReactDOM.render(<App />, document.getElementById('root'));

export default App;
