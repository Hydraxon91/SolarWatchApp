import { useState, useEffect } from "react"
import { json } from "react-router-dom";

export default function RegisterElement(){
    const [dummyText, setDummyText] = useState('test');
    const [email, setEmail] = useState('');
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [role, setRole] = useState('');
    
    const handleSubmit = () => {
        const data = {
            email: email,
            username: username,
            password: password,
            role: role
        }

        fetch('http://localhost:8082/Auth/Register', {
        method: 'POST',
        mode: 'cors',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data)
        })
            .then(res => res.json())
            .then(r => {setDummyText(JSON.stringify(r))})
            .catch(err=>console.error(err))
    };

    return(
        <>
        <div className='registrationForm'>
        <input
                type="text"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)} // Update email state on change
            />
            <input
                type="text"
                placeholder="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)} // Update username state on change
            />
            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)} // Update password state on change
            />
            <input
                type="text"
                placeholder="Role"
                value={role}
                onChange={(e) => setRole(e.target.value)} // Update role state on change
            />
            <button onClick={handleSubmit}>Send POST Request</button>
            {dummyText}
        </div>
        </>
    )
}