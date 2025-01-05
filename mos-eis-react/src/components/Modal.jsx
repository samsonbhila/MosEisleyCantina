import React from "react";
import "../styles/Modal.css";

const Modal = ({ isOpen, title, message, onClose }) => {
  if (!isOpen) return null;

  return (
    <div className="modal-overlay">
      <div className="modal water-wave">
        <h3>{title}</h3>
        <p>{message}</p>
        <button onClick={onClose} className="modal-close-btn">
          Close
        </button>
      </div>
    </div>
  );
};

export default Modal;
