import './App.css';
import { useState, useEffect, React } from 'react';
import ReactDOM from 'react-dom';
import {Elements} from '@stripe/react-stripe-js';
import {loadStripe} from '@stripe/stripe-js';

import CheckoutForm from './checkout-form/index.jsx';


const stripePromise = loadStripe("pk_test_51JWWyOLVagDHTLlfAhdLcFtbDGliiLpCieBDpc71mxZkkymVf5gfJvvUjvdyE8HRNz3XaSTFk1QWw3TN4izstg0F00s0ZrbSLp");

async function getBalance() {
  const response = await fetch('https://api.piggybank.lukejoshuapark.io/balance', {
    method: 'GET',
    mode: 'cors',
    redirect: 'follow',
    referrerPolicy: 'no-referrer',
  }) 

  if (response.status !== 200) {
    return null;
  }
  
  const resJSON = await response.json()
  return resJSON.current_balance;
}

function App() {
  const [balance, setBalance] = useState(0);
  useEffect(() => (async () => { setBalance(await getBalance()) }), []);

  return (
    <div className='page-container'>
      <div className='left-column'>
        <div className='title'>PiggyBank</div>
        <Elements stripe={stripePromise}>
          <CheckoutForm updateBalance={(newBalance) => setBalance(newBalance)}/>
        </Elements>
      </div>
      <div className='right-column'>
        <div className='balance-display'>
          {balance === 0 ? <></> : "$" + balance}
        </div>
      </div>
    </div>
  );
}

ReactDOM.render(<App />, document.getElementById('root'));

export default App;