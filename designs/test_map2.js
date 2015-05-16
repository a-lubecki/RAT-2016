(function(name,data){
 if(typeof onTileMapLoaded === 'undefined') {
  if(typeof TileMaps === 'undefined') TileMaps = {};
  TileMaps[name] = data;
 } else {
  onTileMapLoaded(name,data);
 }})("test_map2",
{ "height":16,
 "layers":[
        {
         "data":[0, 0, 0, 0, 1, 2, 2, 3, 0, 0, 1, 2, 2, 2, 9, 7, 7, 8, 0, 0, 6, 7, 16, 17, 18, 7, 7, 8, 0, 0, 6, 7, 26, 27, 28, 7, 7, 8, 0, 0, 6, 7, 7, 7, 7, 7, 7, 8, 0, 0, 6, 7, 7, 7, 7, 7, 7, 8, 0, 0, 11, 12, 12, 14, 7, 7, 12, 13, 0, 0, 0, 0, 0, 6, 7, 7, 10, 2, 2, 3, 1, 2, 2, 9, 7, 7, 7, 7, 7, 8, 6, 7, 7, 7, 7, 7, 7, 7, 7, 8, 6, 7, 7, 7, 7, 7, 7, 7, 7, 8, 6, 7, 7, 7, 15, 12, 14, 7, 7, 8, 6, 7, 7, 7, 8, 0, 6, 7, 7, 8, 6, 7, 7, 7, 8, 0, 11, 14, 7, 8, 6, 7, 7, 7, 8, 0, 0, 6, 7, 8, 11, 12, 12, 12, 13, 0, 0, 11, 12, 13],
         "height":16,
         "name":"ground",
         "opacity":1,
         "type":"tilelayer",
         "visible":true,
         "width":10,
         "x":0,
         "y":0
        }],
 "nextobjectid":1,
 "orientation":"orthogonal",
 "properties":
    {

    },
 "renderorder":"right-down",
 "tileheight":32,
 "tilesets":[
        {
         "firstgid":1,
         "image":"test_tiles.png",
         "imageheight":96,
         "imagewidth":160,
         "margin":0,
         "name":"Murs Gris",
         "properties":
            {

            },
         "spacing":0,
         "terrains":[
                {
                 "name":"murs",
                 "tile":-1
                }],
         "tileheight":32,
         "tiles":
            {
             "0":
                {
                 "terrain":[0, 0, 0, -1]
                },
             "1":
                {
                 "terrain":[0, 0, -1, -1]
                },
             "10":
                {
                 "terrain":[0, -1, 0, 0]
                },
             "11":
                {
                 "terrain":[-1, -1, 0, 0]
                },
             "12":
                {
                 "terrain":[-1, 0, 0, 0]
                },
             "13":
                {
                 "terrain":[-1, -1, 0, -1]
                },
             "14":
                {
                 "terrain":[-1, -1, -1, 0]
                },
             "2":
                {
                 "terrain":[0, 0, -1, 0]
                },
             "5":
                {
                 "terrain":[0, -1, 0, -1]
                },
             "7":
                {
                 "terrain":[-1, 0, -1, 0]
                },
             "8":
                {
                 "terrain":[0, -1, -1, -1]
                },
             "9":
                {
                 "terrain":[-1, 0, -1, -1]
                }
            },
         "tilewidth":32
        }, 
        {
         "firstgid":16,
         "image":"test_tiles2.png",
         "imageheight":96,
         "imagewidth":160,
         "margin":0,
         "name":"Murs Verts",
         "properties":
            {

            },
         "spacing":0,
         "terrains":[
                {
                 "name":"murs",
                 "tile":-1
                }],
         "tileheight":32,
         "tiles":
            {
             "0":
                {
                 "animation":[
                        {
                         "duration":1000,
                         "tileid":0
                        }, 
                        {
                         "duration":1000,
                         "tileid":14
                        }],
                 "terrain":[0, 0, 0, -1]
                },
             "1":
                {
                 "animation":[
                        {
                         "duration":1000,
                         "tileid":1
                        }, 
                        {
                         "duration":1000,
                         "tileid":11
                        }],
                 "terrain":[0, 0, -1, -1]
                },
             "10":
                {
                 "animation":[
                        {
                         "duration":1000,
                         "tileid":10
                        }, 
                        {
                         "duration":1000,
                         "tileid":9
                        }],
                 "terrain":[0, -1, 0, 0]
                },
             "11":
                {
                 "animation":[
                        {
                         "duration":1000,
                         "tileid":11
                        }, 
                        {
                         "duration":1000,
                         "tileid":1
                        }],
                 "terrain":[-1, -1, 0, 0]
                },
             "12":
                {
                 "animation":[
                        {
                         "duration":1000,
                         "tileid":12
                        }, 
                        {
                         "duration":1000,
                         "tileid":8
                        }],
                 "objectgroup":
                    {
                     "draworder":"index",
                     "height":0,
                     "name":"",
                     "objects":[],
                     "opacity":1,
                     "type":"objectgroup",
                     "visible":true,
                     "width":0,
                     "x":0,
                     "y":0
                    },
                 "terrain":[-1, 0, 0, 0]
                },
             "13":
                {
                 "terrain":[-1, -1, 0, -1]
                },
             "14":
                {
                 "terrain":[-1, -1, -1, 0]
                },
             "2":
                {
                 "animation":[
                        {
                         "duration":1000,
                         "tileid":2
                        }, 
                        {
                         "duration":1000,
                         "tileid":13
                        }],
                 "terrain":[0, 0, -1, 0]
                },
             "5":
                {
                 "animation":[
                        {
                         "duration":1000,
                         "tileid":5
                        }, 
                        {
                         "duration":1000,
                         "tileid":7
                        }],
                 "terrain":[0, -1, 0, -1]
                },
             "7":
                {
                 "animation":[
                        {
                         "duration":1000,
                         "tileid":7
                        }, 
                        {
                         "duration":1000,
                         "tileid":5
                        }],
                 "terrain":[-1, 0, -1, 0]
                },
             "8":
                {
                 "terrain":[0, -1, -1, -1]
                },
             "9":
                {
                 "terrain":[-1, 0, -1, -1]
                }
            },
         "tilewidth":32
        }],
 "tilewidth":32,
 "version":1,
 "width":10
});