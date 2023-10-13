import { useState, useEffect } from "react"
import "../styles/login.css";
import jwt from 'jwt-decode';
import { Link, useNavigate } from 'react-router-dom';
import SuccessfullElement from "./SuccessfullElement";

export default function LoginElement({cookies, setUser}){
    //console.log(user);
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [response, setResponse] = useState(null);
    const [emailInputClass, setEmailInputClass] = useState('login-inputbox');
    const [passwordInputClass, setPasswordInputClass] = useState('login-inputbox');
    const [showSuccessMessage, setShowSuccessMessage] = useState(false);
    const navigate = useNavigate();
    
    const login = (jwt_token) => {
        const decoded = jwt(jwt_token);
        
        setUser(decoded);
        const expirationTimestamp = parseInt(decoded.exp, 10);
        //console.log(decoded)
        console.log(jwt_token)
        const expirationDate = new Date(expirationTimestamp * 1000)
        //console.log(expirationDate);
        cookies.set("jwt_authorization", jwt_token, {expires: expirationDate});
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
                setResponse(r);
            })
            .catch(err=>console.error(err))
    };

    const InputClick = () => {
        setEmailInputClass('login-inputbox');
        setPasswordInputClass('login-inputbox');
    }

    useEffect(()=>{

    },[emailInputClass, passwordInputClass])
    
    useEffect(()=>{
        if (response) {
            if (response?.token) {
                login(response.token);
                setShowSuccessMessage(true);
                setTimeout(() => {
                    setShowSuccessMessage(false);
                    navigate("/");
                }, 3000);
            }
            else{
                if (response['Bad credentials'][0] === 'Invalid email') {
                    setEmailInputClass('login-inputbox wrong-credential');
                }
                else{
                    setPasswordInputClass('login-inputbox wrong-credential');
                }
            }
        }
        
    },[response])

    return(
        <div className='login-form'>
            {showSuccessMessage && (
                <>
                    <SuccessfullElement message={"Successfully logged in"}/>
                    <div className="login-successoverlay" />
                </>
            )}
            <div>
                <form>
                    <h2 className="login-text">Login</h2>
                    <div className="login-inputboxholder">
                        <div className={emailInputClass}>
                            <input type="text" required onClick={InputClick} onChange={(e) => setEmail(e.target.value)}></input>
                            <label for="emailInput">Email</label>
                            <h3 className="invalid-email-text">invalid email</h3>
                        </div>
                        <div className={passwordInputClass}>
                            <input type="password" required onClick={InputClick} onChange={(e) => setPassword(e.target.value)}></input>
                            <label for="">Password</label>
                            <h3 className="invalid-password-text">invalid password</h3>
                        </div>
                    </div>
                    <div className="forget">
                            <a href="https://www.youtube.com/watch?v=dQw4w9WgXcQ"> Forgot Password?</a>
                    </div>
                    <button type="button" className="login-button" onClick={handleSubmit}>Login</button>
                    <div className="register">
                        <p>
                            Don't have an account? 
                            <Link to="/registration">
                                <a> Register here!</a>
                            </Link>
                        </p>
                    </div>
                </form>
            </div>
        </div>
    )
}