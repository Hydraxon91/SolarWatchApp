import { useState, useEffect } from "react"
import { json } from "react-router-dom";
import "../styles/register.css";
import { Link, useNavigate } from 'react-router-dom';
import SuccessfullElement from "./SuccessfullElement";

export default function RegisterElement(){
    const [email, setEmail] = useState(null);
    const [username, setUsername] = useState(null);
    const [password, setPassword] = useState(null);
    const [role, setRole] = useState(null);

    const [response, setResponse] = useState(null);
    const [emailInputClass, setEmailInputClass] = useState('register-inputbox');
    const [passwordInputClass, setPasswordInputClass] = useState('register-inputbox');
    const [userInputClass, setUserInputClass] = useState("register-inputbox");
    const [roleDropdownClass, setRoleDropdownClass] = useState("role-dropdown");
    
    const [showSuccessMessage, setShowSuccessMessage] = useState(false);
    const navigate = useNavigate();
    
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
            .then(r => {setResponse(r)})
            .catch(err=>console.error(err))
    };

    const InputClick = () => {
        setEmailInputClass('register-inputbox');
        setPasswordInputClass('register-inputbox');
        setUserInputClass('register-inputbox');
        setRoleDropdownClass('role-dropdown');
    }

    useEffect(()=>{

    },[emailInputClass, passwordInputClass, userInputClass, roleDropdownClass])
    
    useEffect(()=>{
        if (response?.DuplicateEmail || response?.errors?.Email) {
            setEmailInputClass("register-inputbox wrong-credential")
        }
        if (response?.DuplicateUserName || response?.errors?.Username) {
            setUserInputClass("register-inputbox wrong-credential")
        }
        if (response?.PasswordTooShort || response?.errors?.Password) {
            setPasswordInputClass("register-inputbox wrong-credential")
        }
        if (response?.errors?.Role) {
            setRoleDropdownClass("role-dropdown wrong-credential");
        }
        if (response?.success) {
            setShowSuccessMessage(true);
            setTimeout(() => {
                setShowSuccessMessage(false);
                navigate("/");
            }, 3000);
        }
        //{Role: Array(1), Email: Array(1), Password: Array(1), Username: Array(1)}
        
    },[response])

    return(
        <div className='register-form'>
            {showSuccessMessage && (
                <>
                    <SuccessfullElement message={"Successfully registered"}/>
                    <div className="register-successoverlay" />
                </>
            )}
            <div>
                <form>
                    <h2 className="register-text">Register</h2>
                    <div className="inputboxholder">
                        <div className={emailInputClass}>
                            <input type="text" required onClick={InputClick} onChange={(e) => setEmail(e.target.value)}></input>
                            <label for="">Email</label>
                            <h3 className="invalid-email-text">invalid email or already in use</h3>
                        </div>
                        <div className={userInputClass}>
                            <input type="text" required onClick={InputClick} onChange={(e) => setUsername(e.target.value)}/>
                            <label for="">Username</label>
                            <h3 className="invalid-username-text">invalid username or already in use</h3>
                        </div>
                        <div className={passwordInputClass}>
                            <input type="password" required onClick={InputClick} onChange={(e) => setPassword(e.target.value)}></input>
                            <label for="">Password</label>
                            <h3 className="invalid-password-text">should be at least 6 characters long</h3>
                        </div>
                        <div className={roleDropdownClass}>
                            <select onChange={(e) => setRole(e.target.value)} onClick={InputClick}>
                                <option className="testSelect">Select Role</option>
                                <option className="testSelect" value="User">User</option>
                                <option className="testSelect" value="Admin">Admin</option>
                            </select>
                            <h3 className="invalid-role-text">please select a role</h3>
                        </div>
                    </div>
                    <button type="button" className="register-button" onClick={handleSubmit}>Register</button>
                    <div className="login">
                        <p>
                            Already have an account? 
                            <Link to="/login">
                                <a> Login here!</a>
                            </Link>
                        </p>
                    </div>
                </form>
            </div>
        </div>
    )
}