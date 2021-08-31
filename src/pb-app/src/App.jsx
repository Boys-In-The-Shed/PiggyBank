import './App.css';
import React from 'react';
import ReactDOM from 'react-dom';
import {Elements} from '@stripe/react-stripe-js';
import {loadStripe} from '@stripe/stripe-js';

import CheckoutForm from './checkout-form/index.jsx';

const stripePromise = loadStripe("pk_test_bxJuE0fpBxauHmThIvNnWtDt");

function App() {
  return (
    <div className='page-container'>
      <div className='left-column'>
        <div className='title'>Piggy Bank</div>
        <Elements stripe={stripePromise}>
          <CheckoutForm />
        </Elements>
      </div>
      <div className='right-column'>
        
      </div>
    </div>
  
  );
}

ReactDOM.render(<App />, document.getElementById('root'));

export default App;
