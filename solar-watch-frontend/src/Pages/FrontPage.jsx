import React from 'react';
import { Link } from 'react-router-dom';

export default function FrontPage(){

    return(
           
        <div>
            <h1>Welcome to the Front Page</h1>
            <Link to="/registration">
                <button>Register</button>
            </Link>
            <Link to="/login">
                <button>Login</button>
            </Link>
        </div>
    
        )
    }