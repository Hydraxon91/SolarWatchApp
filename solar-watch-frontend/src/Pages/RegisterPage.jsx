import RegisterElement from "../Components/RegisterElement";
import { Link } from 'react-router-dom';
import "../styles/lotty.css";

export default function RegisterPage(){

    return(
        <div className="curr-page">
            <div className="page-element-container">
                <RegisterElement></RegisterElement>
                <Link to="/">
                    <button>Frontpage</button>
                </Link>
            </div>
        </div>
    
        )
    }