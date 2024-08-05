import "./App.css";
import { useState } from "react";

function App() {
  const [x1, setX1] = useState(0);
  const [y1, setY1] = useState(0);
  const [x2, setX2] = useState(0);
  const [y2, setY2] = useState(0);

  const [rect, setRect] = useState({
    x: 0,
    y: 0,
    width: 0,
    height: 0,
  });

  const [rectangles, setRectangles] = useState([]);

  const handleSubmit = (event) => {
    event.preventDefault();
    fetchIntersections();
  };

  function showRectangles(data) {
    setRectangles(data);
  }

  const handleAddRect = () => {
    console.log(rect);
    addRectangle(rect);
  };

  const handleClear = () => {
    if (window.confirm("Delete the item?")) {
      removeAll();
    }
  };

  function removeAll() {
    fetch("http://localhost:16635/clear", {
      method: "POST",
    })
      .then((response) => {
        if (!response.ok) {
          return response.json().then((json) => {
            if (json.message !== undefined) {
              alert(json.message);
            }
          });
        }
        return response.json();
      })
      .then((data) => {})
      .catch((error) => {
        console.error(error);
      });
  }

  function addRectangle(rect) {
    fetch("http://localhost:16635/addRectangle", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        id: {},
        x: rect.x,
        y: rect.y,
        width: rect.width,
        height: rect.height,
      }),
    })
      .then((response) => {
        if (!response.ok) {
          return response.json().then((json) => {
            if (json.message !== undefined) {
              alert(json.message);
            }
          });
        }
        return response.json();
      })
      .then((data) => {})
      .catch((error) => {
        console.error(error);
      });
  }

  function fetchIntersections() {
    fetch("http://localhost:16635/findIntersectingRectangles", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify([
        {
          x: x1,
          y: y1,
        },
        {
          x: x2,
          y: y2,
        },
      ]),
    })
      .then((response) => {
        if (!response.ok) {
          return response.json().then((json) => {
            if (json.message !== undefined) {
              alert(json.message);
            }
          });
        }
        return response.json();
      })
      .then((data) => {
        showRectangles(data);
      })
      .catch((error) => {
        console.error(error);
      });
  }

  return (
    <>
      <div className="App">
        <header className="App-header">
          <p>Segment Rectangle Intersection App</p>
        </header>
      </div>
      <div className="main_page">
        <div className="coord_block">
          <label className="title">Enter rectangle coordinates.</label>
          <label>X1 Coordinate</label>
          <input type="number" placeholder="x" value={rect.x} onChange={(e) => setRect({ ...rect, x: e.target.value })}></input>
          <label>Y1 Coordinate</label>
          <input type="number" placeholder="y" value={rect.y} onChange={(e) => setRect({ ...rect, y: e.target.value })}></input>
          <label>Width</label>
          <input type="number" placeholder="width" value={rect.width} onChange={(e) => setRect({ ...rect, width: e.target.value })}></input>
          <label>Height</label>
          <input type="number" placeholder="height" value={rect.height} onChange={(e) => setRect({ ...rect, height: e.target.value })}></input>
          <button onClick={(e) => handleAddRect(e)}>Submit</button>
          <button onClick={(e) => handleClear(e)}>Clear up storage</button>
        </div>
        <div className="coord_block">
          <label className="title">Enter line coordinates.</label>
          <label>X1 Coordinate</label>
          <input type="number" placeholder="x" value={x1} onChange={(e) => setX1(e.target.value)}></input>
          <label>Y1 Coordinate</label>
          <input type="number" placeholder="y" value={y1} onChange={(e) => setY1(e.target.value)}></input>
          <label>X2 Coordinate</label>
          <input type="number" placeholder="x" value={x2} onChange={(e) => setX2(e.target.value)}></input>
          <label>Y2 Coordinate</label>
          <input type="number" placeholder="y" value={y2} onChange={(e) => setY2(e.target.value)}></input>
          <button onClick={(e) => handleSubmit(e)}>Submit</button>
        </div>
        <div>
          {rectangles &&
            rectangles.map((rect, i) => (
              <div className="rectangles">
                <div className="rect" key={i}>
                  <label>Rectangle {i + 1} :</label>
                  <div>
                    <label>X: </label>
                    <label>{rect.x}</label>
                  </div>
                  <div>
                    <label>Y: </label>
                    <label>{rect.y}</label>
                  </div>
                  <div>
                    <label>Width: </label>
                    <label>{rect.width}</label>
                  </div>
                  <div>
                    <label>Height: </label>
                    <label>{rect.height}</label>
                  </div>
                </div>
              </div>
            ))}
        </div>
      </div>
    </>
  );
}

export default App;
