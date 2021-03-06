import React from "react";
import style from "./_style.css";
import Octicon from "../Octicon";

const Header = ({ changeMobileView, toggleTheme }) => (
  <header className={style.topBar}>
    <div className={style.headerContainer}>
      <h1>Message Board</h1>
      <button
        className={style.navButton}
        onClick={changeMobileView("messages")}
      >
        Messages
      </button>
      <button className={style.navButton} onClick={changeMobileView("ranking")}>
        Ranking
      </button>
      <button
        className={style.iconButton}
        onClick={toggleTheme}
        title="Toggle theme"
      >
        <Octicon ico="paintcan" />
      </button>
      <a
        className={style.iconButton}
        href="https://github.com/edsondewes/message-board"
        title="Visit GitHub page"
      >
        <Octicon ico="mark-github" />
      </a>
    </div>
  </header>
);

export default Header;
