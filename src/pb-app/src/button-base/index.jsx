import "./button-base.css";

const ButtonBase = ({children, userClick}) => {
    return (
        <div className="buttonBase" onClick={userClick}>{children}</div>
    );
};

export default ButtonBase;