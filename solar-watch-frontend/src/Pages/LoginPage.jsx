import LoginElement from "../Components/LoginElement";
import { Link } from 'react-router-dom';
import "../styles/lotty.css";

export default function LoginPage(props){

    return(
        <div className="curr-page">
            <div className="page-element-container">
                <LoginElement cookies={props.cookies} setUser={props.setUser} user={props.user}></LoginElement>
                <Link to="/">
                    <button>Frontpage</button>
                </Link>
            </div>
        </div>   
        )
    }