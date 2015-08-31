module.exports = function (grunt) {

    // load the task
    grunt.loadNpmTasks("grunt-ts");
    grunt.loadNpmTasks('grunt-nuget-install');

    // Configure grunt here
    grunt.initConfig({
        ts: {
            dev: {                          // a particular target
                src: ["app/**/*.ts"], // The source typescript files, http://gruntjs.com/configuring-tasks#files
                html: ["app/**/*.html"], // The source html files, https://github.com/basarat/grunt-ts#html-2-typescript-support
                reference: "./app/reference.ts",  // If specified, generate this file that you can use for your reference management
                out: 'app/out.js',         // If specified, generate an out.js file which is the merged js file
                watch: 'app'
            },
        },
        nuget_install: {
            //file: 'Client.Project.sln',
        }
    });

    grunt.registerTask("default", ["ts:dev"]);
}