import React from "react";
import VoteContainer from "./VoteContainer";
import { message as messageClass } from "./_style.css";

const Message = ({ id, created, text }) => {
  const dt = new Date(created).toLocaleString("en-US");

  return (
    <article className={messageClass}>
      <p>{text}</p>
      <footer>
        <VoteContainer subjectId={id} />
        <time dateTime={dt}>{dt}</time>
      </footer>
    </article>
  );
};

export default Message;
