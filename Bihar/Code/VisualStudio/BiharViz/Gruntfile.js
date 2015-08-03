module.exports = function (grunt) {
    "use strict";

    grunt.initConfig({
        ts: {
            dev: {
                src: ["app/*.ts"],          // The source typescript files, http://gruntjs.com/configuring-tasks#files
                reference: 'app/reference.ts', // If specified, generate this file that you can use for your reference management
                out: 'app/out.js',             // If specified, generate an out.js file which is the merged js file
                watch: 'app'                  // If specified, watches this directory for changes, and re-runs the current target
            }
        }
    });

    grunt.loadNpmTasks("grunt-ts");
    grunt.registerTask("default", ["ts:dev"]);
};

