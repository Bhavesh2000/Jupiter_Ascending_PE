import React from "react";
import {Link} from "react-router-dom";

function Header(props){

    const isLoggedIn = props.isLoggedIn;

    const handleLogout=()=>{
        props.callBack(false)
    }

    return(
        <div>
            <header>
                   {/*<h3>Trainer's Calender</h3>*/}
                <Link  style={{textDecoration:"none",color:"black"}} to="/calender">Trainer's Calender</Link>
                
                <div className="header-right">
                    <Link style={{textDecoration:"none",color:"black"}} to="/">Home </Link>
                    <Link style={{textDecoration:"none",color:"black"}} to="/about">About us</Link>
                    
                    {isLoggedIn? <>
                    <Link style={{textDecoration:"none",color:"black"}} to="/dashboard">Dashboard</Link>
                    <Link style={{textDecoration:"none",color:"black"}} to="/addTrainer">Add Trainer</Link>
                    <Link style={{textDecoration:"none", color:"black"}} to="/" onClick={handleLogout}>Logout</Link>
                    </>:
                    <Link style={{textDecoration:"none",color:"black"}} to="login">Login</Link>}
                         

                </div>

            </header>
        </div>
    )
}

export default Header;