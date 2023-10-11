import { useState, useEffect } from "react"
import { json } from "react-router-dom";
import jwt from 'jwt-decode';


export default function LoginElement({cookies, user, setUser}){
    //console.log(user);
    const [token, setToken] = useState('test');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const logout = () => {
        setUser(null);
        setToken(null);
        cookies.remove("jwt_authorization");
    }
    
    const login = (jwt_token) => {
        const decoded = jwt(jwt_token);
        
        setUser(decoded);
        const expirationTimestamp = parseInt(decoded.exp, 10);
        //console.log(decoded)
        //console.log(jwt_token)
        const expirationDate = new Date(expirationTimestamp * 1000)
        //console.log(expirationDate);
        cookies.set("jwt_authorization", jwt_token, {
            path: '/', expires: expirationDate,
        });
        setToken("worked");
    }

    const handleSubmit = () => {
        const data = {
            email: email,
            password: password,
        }
        
        fetch('http://localhost:8082/Auth/Login', {
        method: 'POST',
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data)
        })
            .then(res => res.json())
            .then(r => {
                if (r.token) {
                    setToken(JSON.stringify("running login"));
                    login(r.token);
                    
                }
                else {setToken("pail")}
            })
            .catch(err=>console.error(err))
    };

    return(
        <>
        <div className='loginForm'>
            {
                user ? (
                    <div>
                        <p>Welcome {user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]}!</p>
                        <button onClick={logout}>Logout</button>
                    </div>
                ) : (
                <div>
                    <input
                        type="text"
                        placeholder="Email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)} // Update email state on change
                    />
                    <input
                        type="password"
                        placeholder="Password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)} // Update password state on change
                    />
                    <button onClick={handleSubmit}>Send POST Request</button>
                    {token}
                </div>
                )
            }
        
        </div>
        </>
    )
}